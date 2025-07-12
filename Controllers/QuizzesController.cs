using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = "Learner,Admin")]
    public class QuizzesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IQuizQuestionsService _questionsService;

        public QuizzesController(AppDbContext context, IQuizQuestionsService questionsService)
        {
            _context = context;
            _questionsService = questionsService;
        }

         public async Task<IActionResult> Details(int id)
    {
        var quiz = await _context.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quiz == null) return NotFound();

        return View(quiz);
    }

        // GET: /Quizzes/Start/5
        [HttpGet]
        public async Task<IActionResult> Start(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
                return View("NotFound");

            var vm = new TakeQuizVM
            {
                QuizId = quiz.Id,
                QuizTitle = quiz.Title,
                Questions = quiz.Questions.Select(q => new QuizQuestionVM
                {
                    QuestionId = q.Id,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType, // <-- THIS
                    Options = q.Options.Select(o => new QuizOptionVM
                    {
                        OptionId = o.Id,
                        OptionText = o.OptionText,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                }).ToList()
            };

            return View(vm); // Trả về View để chọn đáp án
        }

        // POST: /Quizzes/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(TakeQuizVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int totalQuestions = model.Questions.Count;
            int correctAnswers = 0;
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == model.QuizId); // take quiz

            if (quiz == null) return NotFound();

            var result = new QuizResult
            {
                QuizId = model.QuizId,
                UserId = userId,
                SubmittedAt = DateTime.UtcNow,
                TotalQuestions = totalQuestions,
                Details = new List<QuizResultDetail>()
            };

            foreach (var question in model.Questions)
            {
                bool isCorrect = false;

                if (question.QuestionType == QuestionType.SingleChoice)
                {
                    if (question.SelectedOptionId != null)
                    {
                        var selectedOption = await _context.QuizOptions
                            .FirstOrDefaultAsync(o => o.Id == question.SelectedOptionId);
                        isCorrect = selectedOption?.IsCorrect ?? false;
                        if (isCorrect) correctAnswers++;

                        result.Details.Add(new QuizResultDetail
                        {
                            QuestionId = question.QuestionId,
                            SelectedOptionId = selectedOption?.Id,
                            IsCorrect = isCorrect
                        });
                    }
                }
                else if (question.QuestionType == QuestionType.MultipleChoice)
                {
                    var selectedIds = question.SelectedOptionIds ?? new List<int>();
                    var correctOptionIds = await _context.QuizOptions
                        .Where(o => o.QuestionId == question.QuestionId && o.IsCorrect)
                        .Select(o => o.Id)
                        .ToListAsync();

                    isCorrect = selectedIds.Count == correctOptionIds.Count &&
                                !selectedIds.Except(correctOptionIds).Any();

                    if (isCorrect) correctAnswers++;

                    result.Details.Add(new QuizResultDetail
                    {
                        QuestionId = question.QuestionId,
                        // Lưu null nếu nhiều lựa chọn (nhiều sẽ lưu riêng bảng detail sau này nếu muốn)
                        SelectedOptionId = null,
                        IsCorrect = isCorrect
                    });
                }

                // Gán lại IsCorrect để hiển thị UI
                foreach (var opt in question.Options)
                {
                    var correct = await _context.QuizOptions.FindAsync(opt.OptionId);
                    opt.IsCorrect = correct?.IsCorrect ?? false;
                }
            }


            result.Score = correctAnswers;
            _context.QuizResults.Add(result);
            await _context.SaveChangesAsync();

            model.CourseId = quiz.CourseId; // add courseId to model for result view

            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.CorrectAnswers = correctAnswers;
            ViewBag.ScorePercent = (int)((double)correctAnswers / totalQuestions * 100);

            return View("Result", model);
        }
        public async Task<IActionResult> Manage(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null) return NotFound();

            ViewData["QuizTitle"] = quiz.Title;
            ViewData["CourseId"] = quiz.CourseId;

            return View(quiz);
        }

        [HttpGet]
        [AllowAnonymous] // Cho phép mọi user xem trước
        public async Task<IActionResult> Preview(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null) return NotFound();

            var vm = new QuizPreviewVM
            {
                QuizTitle = quiz.Title,
                Questions = quiz.Questions.Select(q => new QuizQuestionVM
                {
                    QuestionText = q.QuestionText,
                    Options = q.Options.Select(o => new QuizOptionVM
                    {
                        OptionText = o.OptionText,
                        IsCorrect = false // nếu chỉ preview → không tiết lộ đáp án
                    }).ToList()
                }).ToList()
            };

            ViewData["CourseId"] = quiz.CourseId;
            return View(vm);
        }

    }
}

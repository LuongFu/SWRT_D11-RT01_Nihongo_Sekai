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
        private readonly IAIExplanationService _aiService;
        public QuizzesController(AppDbContext context, IQuizQuestionsService questionsService, IAIExplanationService aiService)
        {
            _context = context;
            _questionsService = questionsService;
            _aiService = aiService;
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

            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == model.QuizId);

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

                // Lấy câu hỏi từ DB để có danh sách option chính xác
                var dbQuestion = quiz.Questions.FirstOrDefault(q => q.Id == question.QuestionId);
                if (dbQuestion == null) continue;

                // Lấy danh sách đáp án đúng
                var correctOptions = dbQuestion.Options.Where(o => o.IsCorrect).ToList();
                string correctAnswerText = string.Join(", ", correctOptions.Select(o => o.OptionText));

                if (question.QuestionType == QuestionType.SingleChoice)
                {
                    if (question.SelectedOptionId != null)
                    {
                        var selectedOption = dbQuestion.Options.FirstOrDefault(o => o.Id == question.SelectedOptionId);
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
                    var correctOptionIds = correctOptions.Select(o => o.Id).ToList();

                    isCorrect = selectedIds.Count == correctOptionIds.Count &&
                                !selectedIds.Except(correctOptionIds).Any();

                    if (isCorrect) correctAnswers++;

                    result.Details.Add(new QuizResultDetail
                    {
                        QuestionId = question.QuestionId,
                        SelectedOptionId = null,
                        IsCorrect = isCorrect
                    });
                }

                // Gọi AI để giải thích nếu trả lời sai
                if (!isCorrect)
                {
                    var userAnswerText = string.Join(", ", question.Options
                        .Where(o => question.SelectedOptionIds?.Contains(o.OptionId) == true || o.OptionId == question.SelectedOptionId)
                        .Select(o => o.OptionText));

                    try
                    {
                        question.AIExplanation = await _aiService.GetExplanationAsync(
                            question.QuestionText,
                            userAnswerText,
                            correctAnswerText
                        );
                    }
                    catch (Exception ex)
                    {
                        question.AIExplanation = $"[AI Error]: {ex.Message}";
                    }
                }

                // Gán lại IsCorrect cho UI
                foreach (var opt in question.Options)
                {
                    opt.IsCorrect = correctOptions.Any(co => co.Id == opt.OptionId);
                }
            }

            result.Score = correctAnswers;
            _context.QuizResults.Add(result);
            await _context.SaveChangesAsync();

            model.CourseId = quiz.CourseId; // add courseId to model for result view

            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.CorrectAnswers = correctAnswers;
            ViewBag.ScorePercent = (int)((double)correctAnswers / totalQuestions * 100);

            if (ViewBag.ScorePercent >= 80)
            {
                await MarkContentAsCompleted(quiz.CourseId, quiz.Id, userId);
            }

            return View("Result", model);
        }

        private async Task MarkContentAsCompleted(int courseId, int contentItemId, string userId)
        {
            var existing = await _context.CourseContentProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId &&
                                          p.CourseId == courseId &&
                                          p.ContentItemId == contentItemId);

            if (existing == null)
            {
                _context.CourseContentProgresses.Add(new CourseContentProgress
                {
                    UserId = userId,
                    CourseId = courseId,
                    ContentItemId = contentItemId,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow
                });
            }
            else
            {
                existing.IsCompleted = true;
                existing.CompletedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NihongoSekaiPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data.ViewModels;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = "Learner,Admin")]
    public class QuizzesController : Controller
    {
        private readonly AppDbContext _context;

        public QuizzesController(AppDbContext context)
        {
            _context = context;
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
                QuizOption selectedOption = null;
                bool isCorrect = false;

                if (question.SelectedOptionId != null)
                {
                    selectedOption = await _context.QuizOptions
                        .Include(o => o.Question)
                        .FirstOrDefaultAsync(o => o.Id == question.SelectedOptionId);

                    isCorrect = selectedOption != null && selectedOption.IsCorrect;

                    if (isCorrect)
                        correctAnswers++;
                }

                result.Details.Add(new QuizResultDetail
                {
                    QuestionId = question.QuestionId,
                    SelectedOptionId = selectedOption?.Id,
                    IsCorrect = isCorrect
                });

                // Optional: cập nhật lại IsCorrect trong ViewModel để highlight UI
                foreach (var opt in question.Options)
                {
                    var correct = await _context.QuizOptions.FindAsync(opt.OptionId);
                    opt.IsCorrect = correct?.IsCorrect ?? false;
                }
            }

            result.Score = correctAnswers;
            _context.QuizResults.Add(result);
            await _context.SaveChangesAsync();

            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.CorrectAnswers = correctAnswers;
            ViewBag.ScorePercent = (int)((double)correctAnswers / totalQuestions * 100);

            return View("Result", model);
        }
    }
}

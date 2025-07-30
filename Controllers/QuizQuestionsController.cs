using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuizQuestionsController : Controller
    {
        private readonly IQuizQuestionsService _service;

        public QuizQuestionsController(IQuizQuestionsService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index(int quizId)
        {
            return RedirectToAction("Create", new { quizId });
        }

        // Display form for creating a new question
        [HttpGet]
        public async Task<IActionResult> Create(int quizId)
        {
            var viewModel = await _service.GetQuestionFormAsync(null, quizId);
            return View("Edit", viewModel); // Reuse Edit.cshtml for Create
        }

        // Display form for editing an existing question
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _service.GetQuestionFormAsync(id, 0);
            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        // Handle POST for both create and update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(QuizQuestionFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                if (vm.QuestionType == QuestionType.SingleChoice)
                {
                    int correctCount = vm.Options.Count(o => o.IsCorrect);
                    if (correctCount != 1)
                    {
                        ModelState.AddModelError("", "Single Choice question must have exactly one correct answer.");
                    }
                }
                if (vm.QuestionType == QuestionType.MultipleChoice)
                {
                    int correctCount = vm.Options.Count(o => o.IsCorrect);
                    if (correctCount < 1)
                    {
                        ModelState.AddModelError("", "Multiple Choice question must have at least one correct answer.");
                    }
                }
                return View("Edit", vm);
            }

            await _service.SaveQuestionAsync(vm);

            TempData["SuccessMessage"] = "Đã lưu câu hỏi.";
            return RedirectToAction("Manage", "Quizzes", new { id = vm.QuizId });
        }

        // Confirm delete via GET (optional modal later)
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int quizId)
        {
            await _service.DeleteQuestionAsync(id);
            TempData["SuccessMessage"] = "Đã xóa câu hỏi.";
            return RedirectToAction("Manage", "Quizzes", new { id = quizId });
        }
    }
}

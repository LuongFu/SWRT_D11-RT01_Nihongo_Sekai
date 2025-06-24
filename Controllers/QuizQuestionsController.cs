using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
                return View("Edit", vm);

            await _service.SaveQuestionAsync(vm);

            TempData["SuccessMessage"] = "Question saved successfully.";
            return RedirectToAction("Details", "Quizzes", new { id = vm.QuizId });
        }

        // Confirm delete via GET (optional modal later)
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int quizId)
        {
            await _service.DeleteQuestionAsync(id);
            TempData["SuccessMessage"] = "Question deleted successfully.";
            return RedirectToAction("Details", "Quizzes", new { id = quizId });
        }
    }
}

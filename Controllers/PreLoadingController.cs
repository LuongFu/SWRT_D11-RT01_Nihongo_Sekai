using Microsoft.AspNetCore.Mvc;


namespace JapaneseLearningPlatform.Controllers
{
    public class PreLoadingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return RedirectToAction("Index", "Loading", new { returnUrl = "/Admin/Index" });
        }
        public IActionResult Partner()
        {
            return RedirectToAction("Index", "Loading", new { returnUrl = "/Partners/Index" });
        }

        public IActionResult Learner()
        {
            return RedirectToAction("Index", "Loading", new { returnUrl = "/Learners/Index" });
        }
    }
}

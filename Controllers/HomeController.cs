using Microsoft.AspNetCore.Mvc;


namespace JapaneseLearningPlatform.Controllers
{
    public class HomeController : Controller
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
            return RedirectToAction("Index", "Loading", new { returnUrl = "/Partner/Index" });
        }

        public IActionResult Learner()
        {
            return RedirectToAction("Index", "Loading", new { returnUrl = "/Learner/Index" });
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Controllers
{
    // Nếu muốn URL chỉ /About, /Contact…:
    [Route("[action]")]
    public class SidePagesController : Controller
    {
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Community() => View();
        public IActionResult Partners() => View();
        public IActionResult Blog() => View();
        public IActionResult Careers() => View();
        public IActionResult Press() => View();
        public IActionResult Pricing() => View();
        public IActionResult Help() => View();
        public IActionResult Privacy() => View();
        public IActionResult Terms() => View();
        public IActionResult CookiePolicy() => View();
    }
}

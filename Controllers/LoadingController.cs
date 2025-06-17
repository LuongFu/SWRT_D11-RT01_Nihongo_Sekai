using Microsoft.AspNetCore.Mvc;

namespace NihongoSekaiPlatform.Controllers
{
    public class LoadingController : Controller
    {
        public IActionResult Index(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NihongoSekaiPlatform.Controllers
{
    public class PartnerController : Controller
    {
        // GET: PartnerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PartnerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PartnerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PartnerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PartnerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PartnerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PartnerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PartnerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

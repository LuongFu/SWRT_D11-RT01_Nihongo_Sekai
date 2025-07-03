using JapaneseLearningPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.TotalRevenue = _context.Orders.Sum(o => o.TotalAmount);
            ViewBag.TotalClassrooms = _context.ClassroomInstances.Count();
            ViewBag.RecentOrders = _context.Orders
                                    .Include(o => o.User)
                                    .Include(o => o.OrderItems)
                                    .OrderByDescending(o => o.OrderDate)
                                    .Take(5)
                                    .ToList();

            var monthLabels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            ViewBag.MonthLabels = monthLabels;

            ViewBag.MonthlyRevenue = Enumerable.Range(1, 12)
                .Select(m => _context.Orders.Where(o => o.OrderDate.Month == m).Sum(o => o.TotalAmount))
                .ToList();

            ViewBag.MonthlyOrders = Enumerable.Range(1, 12)
                .Select(m => _context.Orders.Count(o => o.OrderDate.Month == m))
                .ToList();

            return View();
        }

        // GET: AdminController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
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

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
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

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
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

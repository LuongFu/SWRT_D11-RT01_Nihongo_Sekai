using System;
using System.Linq;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(AppDbContext context,
                                 UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: Reports/Submit
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ReportViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Contact", "SidePages");

            var role = User.Identity?.IsAuthenticated == true
                       ? User.IsInRole("Learner") ? "Learner"
                         : User.IsInRole("Partner") ? "Partner"
                         : "Guest"
                       : "Guest";

            var report = new Report
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Subject = vm.Subject,
                OrderNumber = vm.OrderNumber,
                Message = vm.Message,
                Role = role,
                SubmittedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return RedirectToAction("ContactConfirmation", "SidePages");
        }

        // GET: Reports (Learner/Partner)
        [Authorize(Roles = "Learner,Partner")]
        public async Task<IActionResult> Index(int page = 1, string q = "")
        {
            const int pageSize = 10;
            var userEmail = User.Identity!.Name!;
            var query = _context.Reports.Where(r => r.Email == userEmail);

            if (!string.IsNullOrEmpty(q))
                query = query.Where(r => r.Subject.ToString().Contains(q)
                                       || r.Message.Contains(q));

            var total = await query.CountAsync();
            var items = await query
                         .OrderByDescending(r => r.SubmittedAt)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.Q = q;

            return View(items);
        }

        // GET: Reports/AdminIndex (Admin)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex(string subject = "",
                                                     string role = "",
                                                     string status = "",
                                                     int page = 1,
                                                     string q = "")
        {
            const int pageSize = 10;
            var query = _context.Reports.AsQueryable();

            if (Enum.TryParse<ReportSubject>(subject, out var subj))
                query = query.Where(r => r.Subject == subj);
            if (!string.IsNullOrEmpty(role))
                query = query.Where(r => r.Role == role);
            if (status == "Resolved")
                query = query.Where(r => r.IsResolved);
            else if (status == "Unresolved")
                query = query.Where(r => !r.IsResolved);
            if (!string.IsNullOrEmpty(q))
                query = query.Where(r => r.FullName.Contains(q)
                                       || r.Email.Contains(q)
                                       || (r.OrderNumber ?? "").Contains(q)
                                       || r.Message.Contains(q));

            ViewBag.Subjects = await _context.Reports
                                 .GroupBy(r => r.Subject)
                                 .Select(g => new { Key = g.Key, Count = g.Count() })
                                 .ToListAsync();
            ViewBag.Roles = new[] { "Learner", "Partner", "Guest" };
            ViewBag.Statuses = new[] { "Unresolved", "Resolved" };

            var total = await query.CountAsync();
            var items = await query
                         .OrderByDescending(r => r.SubmittedAt)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.Q = q;
            ViewBag.CurrentSubject = subject;
            ViewBag.CurrentRole = role;
            ViewBag.CurrentStatus = status;

            return View("~/Views/Admin/ViewReportsList.cshtml", items);
        }

        // GET: Reports/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var r = await _context.Reports.FindAsync(id);
            if (r == null) return NotFound();

            // Trả về file nằm trong Views/Admin/ViewReportDetails.cshtml
            return View("~/Views/Admin/ViewReportDetails.cshtml", r);
        }

        // POST: Reports/Resolve/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Resolve(int id)
        {
            var r = await _context.Reports.FindAsync(id);
            if (r == null) return NotFound();
            r.IsResolved = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminIndex));
        }
    }
}

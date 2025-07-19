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
using Microsoft.Extensions.Logging;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<ReportsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // POST: Reports/Submit
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ReportViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Log chi tiết các lỗi validation
                foreach (var kv in ModelState)
                {
                    foreach (var err in kv.Value.Errors)
                    {
                        _logger.LogWarning("Field \"{Field}\" invalid: {Error}", kv.Key, err.ErrorMessage);
                    }
                }

                // Lưu thông báo thất bại
                TempData["ToastMessage"] = "Gửi tin nhắn thất bại. Vui lòng kiểm tra lại.";
                TempData["ToastType"] = "error";

                // Trả về view ban đầu (hoặc redirect) để hiển thị validation messages
                return Redirect("/Contact");
            }

            // Xác định vai trò
            var role = User?.Identity?.IsAuthenticated == true
                ? (User.IsInRole("Learner") ? "Learner"
                   : User.IsInRole("Partner") ? "Partner"
                   : "Guest")
                : "Guest";

            var report = new Report
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Subject = vm.Subject,
                OrderNumber = vm.OrderNumber,    // có thể null
                Message = vm.Message,
                Role = role,
                SubmittedAt = DateTime.UtcNow,
                IsResolved = false
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            // Lưu thông báo thành công
            TempData["ToastMessage"] = "Gửi tin nhắn thành công! Chúng tôi sẽ phản hồi sớm.";
            TempData["ToastType"] = "success";

            return Redirect("/Contact");
        }

        // GET: Reports (Learner/Partner)
        [Authorize(Roles = "Learner,Partner")]
        public async Task<IActionResult> Index(int page = 1, string q = "")
        {
            const int pageSize = 10;
            var userEmail = User.Identity!.Name!;
            var query = _context.Reports
                .Where(r => r.Email == userEmail);

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(r =>
                    r.Subject.ToString().Contains(q) ||
                    r.Message.Contains(q));
            }

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

        // GET: Reports/AdminIndex
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex(
            string subject = "",
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
                query = query.Where(r =>
                    r.FullName.Contains(q) ||
                    r.Email.Contains(q) ||
                    (r.OrderNumber ?? "").Contains(q) ||
                    r.Message.Contains(q));

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

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUnresolvedCounts()
        {
            var total = await _context.Reports.CountAsync(r => !r.IsResolved);
            var bySubj = await _context.Reports
                .Where(r => !r.IsResolved)
                .GroupBy(r => r.Subject)
                .Select(g => new {
                    Subject = g.Key.ToString(),
                    Count = g.Count()
                })
                .Where(x => x.Count > 0)
                .ToListAsync();
            return Json(new { total, bySubj });
        }

    }
}

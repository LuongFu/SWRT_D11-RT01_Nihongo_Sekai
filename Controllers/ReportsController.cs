using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReportsController> _logger;

        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;

        // Hard‑code ở đây
        private const string RecaptchaSiteKey = "6LcuZY4rAAAAAOB7URpx0BIssk-U9kmjQZvcBXD_";
        private const string RecaptchaSecretKey = "6LcuZY4rAAAAAMaZYSTeIuTGXJXgyDJWBffJI5zH";

        public ReportsController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<ReportsController> logger,
            IConfiguration config,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _config = config;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            var vm = new ReportViewModel();

            var isAuthenticated = User.Identity?.IsAuthenticated == true;
            var isAdmin = isAuthenticated && User.IsInRole("Admin");
            var isLearner = isAuthenticated && User.IsInRole("Learner");
            var isPartner = isAuthenticated && User.IsInRole("Partner");
            var isGuest = !isAuthenticated;

            ViewBag.IsAdminViewing = isAdmin;
            ViewBag.IsReadonlyUser = isLearner || isPartner;
            ViewBag.IsGuest = isGuest;
            // truyền SiteKey xuống View
            ViewBag.RecaptchaSiteKey = RecaptchaSiteKey;

            if (isLearner || isPartner)
            {
                var appUser = await _userManager.GetUserAsync(User)!;
                //ViewBag.PrefillName = appUser.FullName;
                //ViewBag.PrefillEmail = appUser.Email;
                vm.FullName = appUser.FullName;
                vm.Email = appUser.Email;
            }
            else if (isGuest)
            {
                ViewBag.PrefillName = "";
                ViewBag.PrefillEmail = $"guest_{Guid.NewGuid():N}@example.com";
            }
            else // admin
            {
                ViewBag.PrefillName = "";
                ViewBag.PrefillEmail = "";
            }

            //ModelState.Remove(nameof(vm.FullName));
            //ModelState.Remove(nameof(vm.Email));

            return View("~/Views/SidePages/Contact.cshtml", vm);
        }

        // POST: Reports/Submit
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ReportViewModel vm)
        {
            // 0) Learner/Partner must match profile
            if (User.Identity?.IsAuthenticated == true
                && (User.IsInRole("Learner") || User.IsInRole("Partner")))
            {
                var appUser = await _userManager.GetUserAsync(User)!;
                if (vm.FullName != appUser.FullName)
                    ModelState.AddModelError(nameof(vm.FullName),
                        $"Họ và tên của bạn là: {appUser.FullName}");
                if (vm.Email != appUser.Email)
                    ModelState.AddModelError(nameof(vm.Email),
                        $"Email của bạn là: {appUser.Email}");
            }

            // 1) Verify reCAPTCHA
            var token = vm.RecaptchaToken;
            using var client = new HttpClient();
            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("secret",   RecaptchaSecretKey),
                new KeyValuePair<string,string>("response", token)
            });
            var googleRes = await client.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify", form);
            var captchaResult = await googleRes.Content
                .ReadFromJsonAsync<RecaptchaResponse>();
            if (captchaResult == null || !captchaResult.success)
            {
                ModelState.AddModelError(nameof(vm.RecaptchaToken),
                    "Vui lòng xác minh rằng bạn không phải robot.");
            }

            // 2) Nếu có lỗi, rebuild ViewBag và trả về lại view
            if (!ModelState.IsValid)
            {
                var isAuth = User.Identity?.IsAuthenticated == true;
                var isAdmin = isAuth && User.IsInRole("Admin");
                var isLear = isAuth && User.IsInRole("Learner");
                var isPart = isAuth && User.IsInRole("Partner");
                var isGuest = !isAuth;

                ViewBag.IsAdminViewing = isAdmin;
                ViewBag.IsReadonlyUser = isLear || isPart;
                ViewBag.IsGuest = isGuest;
                ViewBag.RecaptchaSiteKey = RecaptchaSiteKey;

                if (isLear || isPart)
                {
                    var u = await _userManager.GetUserAsync(User)!;
                    ViewBag.PrefillName = u.FullName;
                    ViewBag.PrefillEmail = u.Email;
                }
                else if (isGuest)
                {
                    ViewBag.PrefillName = "";
                    ViewBag.PrefillEmail = $"guest_{Guid.NewGuid():N}@example.com";
                }
                else
                {
                    ViewBag.PrefillName = "";
                    ViewBag.PrefillEmail = "";
                }

                foreach (var kv in ModelState)
                    foreach (var err in kv.Value.Errors)
                        _logger.LogWarning("Field {Field} invalid: {Error}",
                                           kv.Key, err.ErrorMessage);

                TempData["ToastMessage"] = "Gửi tin nhắn thất bại. Vui lòng kiểm tra lại.";
                TempData["ToastType"] = "error";

                return View("~/Views/SidePages/Contact.cshtml", vm);
            }

            // 3) Lưu Report
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
                SubmittedAt = DateTime.UtcNow,
                IsResolved = false
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Gửi tin nhắn thành công! Chúng tôi sẽ phản hồi sớm.";
            TempData["ToastType"] = "success";
            return RedirectToAction("Contact");
        }

        // POCO for Google’s JSON
        public class RecaptchaResponse
        {
            public bool success { get; set; }
            public string challenge_ts { get; set; }
            public string hostname { get; set; }
            public string[]? error_codes { get; set; }
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

            // 1) read feature flag
            bool toastEnabled = _config
               .GetValue<bool>("Reporting:EnableNewReportToast");
            ViewBag.EnableNewReportToast = toastEnabled;

            // 2) compute how many unresolved since last admin visit
            //    (you can adapt “lastVisit” however you persist it)
            if (toastEnabled)
            {
                // e.g. count all unresolved
                ViewBag.NewCount = await _context.Reports
                                          .CountAsync(r => !r.IsResolved);
            }

            if (toastEnabled)
            {
                ViewBag.NewCount = await _context.Reports.CountAsync(r => !r.IsResolved);
                ViewBag.NewReports = await _context.Reports
                    .Where(r => !r.IsResolved)
                    .OrderByDescending(r => r.SubmittedAt)
                    .Take(3)
                    .Select(r => new { r.Id, r.Subject, r.SubmittedAt })
                    .ToListAsync();
            }

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

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(int reportId, string subject, string body)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null) return NotFound();

            // 1) (Tuỳ bạn) đánh dấu đã trả lời
            report.IsResolved = true;
            await _context.SaveChangesAsync();

            // 2) Inject placeholder nếu cần
            //    ví dụ {{ReporterName}}, {{OriginalMessage}}
            body = body
              .Replace("{{ReporterName}}", report.FullName)
              .Replace("{{OriginalMessage}}", report.Message);

            // 3) Gửi email
            await _emailSender.SendEmailAsync(
                report.Email,
                subject,
                body);

            TempData["ToastMessage"] = $"Đã gửi phản hồi cho {report.FullName}";
            TempData["ToastType"] = "success";

            return RedirectToAction(nameof(AdminIndex));                
        }
    }
}


using System;
using System.Linq;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data;                    // AppDbContext
using JapaneseLearningPlatform.Data.Enums;              // PartnerStatus
using JapaneseLearningPlatform.Data.Static;             // UserRoles
using JapaneseLearningPlatform.Data.ViewModels;         // ReviewPartnerVM
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;               // [Authorize]
using Microsoft.AspNetCore.Identity;                    // UserManager<>, SignInManager<>
using Microsoft.AspNetCore.Identity.UI.Services;        // IEmailSender
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;


namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;  // <-- thêm
        private readonly IEmailSender _emailSender;  // <-- thêm
        private readonly IWebHostEnvironment _env;

        public AdminController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
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
                                    .Take(10)
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
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
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
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
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
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
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


        #region —— Quản lý Partner Applications ——

        /// <summary>
        /// Danh sách các hồ sơ Partner đang chờ duyệt
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> PartnerApplications(string filter = "Pending")
        {
            // 1) Parse filter into enum (default to Pending)
            if (!Enum.TryParse<PartnerStatus>(filter, true, out var status))
                status = PartnerStatus.Pending;

            // 2) Base query: include User, specs, docs as needed
            var query = _context.PartnerProfiles
                                .Include(p => p.User)
                                .Include(p => p.Specializations)
                                .Include(p => p.Documents)
                                .Where(p => p.Status == status);

            // 3) If Rejected: only those rejected within last 7 days
            if (status == PartnerStatus.Rejected)
            {
                var cutoff = DateTime.UtcNow.AddDays(-7);
                query = query.Where(p => p.DecisionAt.HasValue
                                      && p.DecisionAt.Value >= cutoff);
            }

            // 4) Execute
            var list = await query.ToListAsync();

            // Pass current filter to ViewData so view can highlight the right tab
            ViewData["CurrentFilter"] = status;
            return View(list);
        }

        /// <summary>
        /// Xem chi tiết một hồ sơ Partner
        /// </summary>
        // GET: /Admin/ReviewPartner/5
        public async Task<IActionResult> ReviewPartner(int id)
        {
            var profile = await _context.PartnerProfiles
                .Include(p => p.User)
                .Include(p => p.Specializations)
                .Include(p => p.Documents)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profile == null) return NotFound();

            var vm = new ReviewPartnerVM
            {
                Id = profile.Id,
                Profile = profile
            };
            return View(vm);
        }

        /// <summary>
        /// Duyệt (approve=true) hoặc từ chối (approve=false) hồ sơ Partner
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePartner(int partnerId, string subject, string body)
        {
            var profile = await _context.PartnerProfiles
                                .Include(p => p.User)
                                .FirstOrDefaultAsync(p => p.Id == partnerId);
            if (profile == null) return NotFound();

            var user = profile.User!;

            // 1) Cập nhật trạng thái trong DB
            profile.Status = PartnerStatus.Approved;
            profile.DecisionAt = DateTime.UtcNow;
            user.IsApproved = true;
            await _context.SaveChangesAsync();

            // 2) Sinh token & callbackUrl để xác nhận email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                nameof(AccountController.Login),  
                "Account",
                new { userId = user.Id, token },
                protocol: Request.Scheme
            );

            // 3) Tiêm callbackUrl vào template đã soạn
            body = body.Replace("{{CallbackUrl}}", callbackUrl);

            // 4) Gửi email
            await _emailSender.SendEmailAsync(user.Email!, subject, body);

            TempData["Success"] = $"Đã phê duyệt {user.FullName} và gửi email xác nhận.";
            return RedirectToAction(nameof(PartnerApplications));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPartner(int partnerId, string subject, string body)
        {
            var profile = await _context.PartnerProfiles
                                .Include(p => p.User)
                                .FirstOrDefaultAsync(p => p.Id == partnerId);
            if (profile == null) return NotFound();

            var user = profile.User!;

            // 1) Cập nhật trạng thái audit
            profile.Status = PartnerStatus.Rejected;
            profile.DecisionAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // 2) Sinh registerUrl
            var registerUrl = Url.Action(
                nameof(AccountController.Register),
                "Account",
                null,
                protocol: Request.Scheme
            );

            // 3) Tiêm {{RegisterUrl}}
            body = body.Replace("{{RegisterUrl}}", registerUrl);

            // 4) Gửi email
            await _emailSender.SendEmailAsync(user.Email!, subject, body);

            // 5) Xóa hồ sơ + user + folder
            _context.PartnerProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            await _userManager.DeleteAsync(user);
            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "partner_docs", user.Id);
            if (Directory.Exists(uploadsDir)) Directory.Delete(uploadsDir, true);

            TempData["Success"] = $"Đã từ chối {user.FullName}, xóa hồ sơ và gửi email.";
            return RedirectToAction(nameof(PartnerApplications));
        }
        #endregion
    }
}

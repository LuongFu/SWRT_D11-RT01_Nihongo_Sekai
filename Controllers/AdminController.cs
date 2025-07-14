using System;
using System.Linq;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data;                    // AppDbContext
using JapaneseLearningPlatform.Data.Enums;              // PartnerStatus
using JapaneseLearningPlatform.Data.Static;             // UserRoles
using JapaneseLearningPlatform.Data.ViewModels;         // ReviewPartnerVM
using JapaneseLearningPlatform.Models;
using JapaneseLearningPlatform.Models.Partner;           // PartnerProfile, PartnerDocument...
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
        public async Task<IActionResult> ApprovePartner(int id)
        {
            var profile = await _context.PartnerProfiles
                              .Include(p => p.User)
                              .FirstOrDefaultAsync(p => p.Id == id);
            if (profile == null) return NotFound();

            profile.Status = PartnerStatus.Approved;
            profile.DecisionAt = DateTime.UtcNow;

            profile.User.IsApproved = true;

            await _context.SaveChangesAsync();

            // Generate confirmation link
            var user = profile.User;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                nameof(AccountController.Login),
                "Account",
                new { userId = user.Id, token },
                protocol: Request.Scheme
            );

            // Send beautifully formatted email
            var subject = "🎉 Your Partner Application Is Approved!";
            var body = $@"<!DOCTYPE html>
            <html><head><meta charset=""UTF-8""/></head>
            <body style=""margin:0;padding:0;background:#f4f4f7;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                <tr><td align=""center"">
                    <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0""
                            style=""background:#ffffff;border-radius:8px;overflow:hidden;"">
                    <!-- Banner -->
                    <tr>
                        <td>
                        <img src=""https://source.unsplash.com/600x200/?celebration,confetti"" 
                                alt=""Congratulations!"" width=""600"" style=""display:block;""/>
                        </td>
                    </tr>
                    <!-- Content -->
                    <tr>
                        <td style=""padding:20px;font-family:Arial,sans-serif;color:#333;"">
                        <img src=""https://source.unsplash.com/80x80/?logo,brand"" 
                                alt=""Nihongo Sekai"" width=""80"" style=""display:block;margin-bottom:20px;""/>
                        <h2 style=""margin:0 0 15px;color:#E53935;font-size:24px;"">
                            Hello {user.FullName},
                        </h2>
                        <p style=""font-size:16px;line-height:1.5;"">
                            Congratulations! Your application as a <strong>Teaching Partner</strong> has been
                            <span style=""color:#22c55e;"">approved</span>.
                        </p>
                        <p style=""text-align:center;margin:30px 0;"">
                            <a href=""{callbackUrl}""
                                style=""display:inline-block;padding:12px 24px;
                                    background:#E53935;color:#fff;text-decoration:none;
                                    border-radius:4px;font-weight:bold;"">
                            Confirm &amp; Log In
                            </a>
                        </p>
                        <p style=""font-size:14px;color:#666;line-height:1.4;"">
                            If the button above doesn’t work, copy &amp; paste this link into your browser:<br/>
                            <a href=""{callbackUrl}"" style=""color:#1a73e8;word-break:break-all;"">
                            {callbackUrl}
                            </a>
                        </p>
                        </td>
                    </tr>
                    <!-- Footer -->
                    <tr>
                        <td style=""background:#f9f9fa;padding:15px;text-align:center;
                                    font-family:Arial,sans-serif;color:#888;font-size:12px;"">
                        <p style=""margin:0 0 10px;"">Follow us</p>
                        <a href=""https://facebook.com/nihongosekai"">
                            <img src=""https://cdn-icons-png.flaticon.com/24/733/733547.png""
                                alt=""FB"" width=""24"" style=""margin:0 5px;""/>
                        </a>
                        <a href=""https://twitter.com/nihongosekai"">
                            <img src=""https://cdn-icons-png.flaticon.com/24/733/733579.png""
                                alt=""TW"" width=""24"" style=""margin:0 5px;""/>
                        </a>
                        <a href=""https://instagram.com/nihongosekai"">
                            <img src=""https://cdn-icons-png.flaticon.com/24/2111/2111463.png""
                                alt=""IG"" width=""24"" style=""margin:0 5px;""/>
                        </a>
                        <p style=""margin-top:10px;"">© {DateTime.UtcNow.Year} Nihongo Sekai</p>
                        </td>
                    </tr>
                    </table>
                </td></tr>
                </table>
            </body>
            </html>";

            await _emailSender.SendEmailAsync(user.Email!, subject, body);
            return RedirectToAction(nameof(PartnerApplications));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPartner(int id)
        {
            // 1) load profile + user
            var profile = await _context.PartnerProfiles
                                .Include(p => p.User)
                                .FirstOrDefaultAsync(p => p.Id == id);
            if (profile == null) return NotFound();

            // 2) mark as Rejected for your audit table
            profile.Status = PartnerStatus.Rejected;
            profile.DecisionAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // 3) send "sorry, not approved" email
            var user = profile.User;
            var subject = "🙏 Your Partner Application Status";
            var registerUrl = Url.Action(nameof(AccountController.Register),
                                         "Account", null, Request.Scheme)!;

            var body = $@"<!DOCTYPE html>
            <html><head><meta charset=""UTF-8""/></head>
            <body style=""margin:0;padding:0;background:#f4f4f7;"">
              <table width=""100%"" cellpadding=""0"" cellspacing=""0"">
                <tr><td align=""center"">
                  <table width=""600"" cellpadding=""0"" cellspacing=""0"" 
                         style=""background:#ffffff;border-radius:8px;overflow:hidden;"">
                    <!-- Banner -->
                    <tr>
                      <td>
                        <img src=""https://source.unsplash.com/600x200/?sorry,stamp"" 
                             alt=""Application Not Approved"" width=""600"" 
                             style=""display:block;""/>
                      </td>
                    </tr>
                    <!-- Content -->
                    <tr>
                      <td style=""padding:20px;font-family:Arial,sans-serif;color:#333;"">
                        <h2 style=""margin:0 0 15px;color:#D32F2F;font-size:24px;"">
                          We’re Sorry, {user.FullName}
                        </h2>
                        <p style=""font-size:16px;line-height:1.5;"">
                          Unfortunately, your application to become a Teaching Partner was 
                          <strong style=""color:#D32F2F;"">not approved</strong> at this time.
                        </p>
                        <p style=""font-size:16px;line-height:1.5;margin-top:20px;"">
                          You’re welcome to gain more experience and 
                          <a href=""{registerUrl}"" style=""color:#1a73e8;"">
                            apply again
                          </a> when you’re ready.
                        </p>
                      </td>
                    </tr>
                    <!-- Footer -->
                    <tr>
                      <td style=""background:#f9f9fa;padding:15px;
                                 text-align:center;font-family:Arial,sans-serif;
                                 color:#888;font-size:12px;"">
                        <p style=""margin:0;"">© {DateTime.UtcNow.Year} Nihongo Sekai</p>
                      </td>
                    </tr>
                  </table>
                </td></tr>
              </table>
            </body>
            </html>";

            await _emailSender.SendEmailAsync(user.Email!, subject, body);

            // 2) Remove the profile (documents go with it via cascade)
            _context.PartnerProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            // 3) Delete the user account
            await _userManager.DeleteAsync(profile.User);

            // 4) (Optional) Delete their physical uploads folder
            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "partners", profile.User.Id);
            if (Directory.Exists(uploadsDir))
                Directory.Delete(uploadsDir, recursive: true);

            return RedirectToAction(nameof(PartnerApplications));
        }
        #endregion
    }
}

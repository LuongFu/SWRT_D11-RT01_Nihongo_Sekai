using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Models.Partner;

namespace JapaneseLearningPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context,
            IEmailSender emailSender,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _env = env;
        }

        // GET: /Account/Register
        public IActionResult Register() => View(new RegisterVM());

        // GET: /Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // GET: /Account/RegisterCompleted
        [HttpGet]
        public IActionResult RegisterCompleted()
        {
            return View();
        }

        //get: /Account/ForgotPasswordConfirmation

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            if (await _userManager.FindByEmailAsync(registerVM.EmailAddress!) != null)
            {
                TempData["Error"] = "Email này đã được đăng ký. Hãy tiến hành đăng nhập.";
                return View(registerVM);
            }

            var newUser = new ApplicationUser
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
                EmailConfirmed = false,
                IsApproved = !registerVM.ApplyAsPartner,
                Role = registerVM.ApplyAsPartner ? "Partner" : "Learner"
            };

            var createResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(registerVM);
            }
                       
            if (registerVM.ApplyAsPartner)
            {
                // 1) Tạo PartnerProfile
                var profile = new PartnerProfile
                {
                    UserId = newUser.Id,
                    YearsOfExperience = registerVM.YearsOfExperience!.Value,
                    CreatedAt = DateTime.UtcNow    // ← gán ngày giờ hiện tại
                };
                _context.PartnerProfiles.Add(profile);
                await _context.SaveChangesAsync();

                // 2) Tạo PartnerSpecialization
                foreach (var spec in registerVM.Specializations ?? Enumerable.Empty<SpecializationType>())
                {
                    _context.PartnerSpecializations.Add(new PartnerSpecialization
                    {
                        PartnerProfileId = profile.Id,
                        Specialization = spec
                    });
                }
                await _context.SaveChangesAsync();

                // 3) Upload & lưu PartnerDocument (nhiều file)
                var userFolder = newUser.Id;
                // Dùng partner_docs thay vì partners
                var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", "partner_docs", userFolder);
                Directory.CreateDirectory(uploadsRoot);

                foreach (var f in registerVM.PartnerDocument)
                {
                    var ext = Path.GetExtension(f.FileName).ToLower();
                    if (new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" }.Contains(ext)
                        && f.Length <= 10 * 1024 * 1024)
                    {
                        var fn = $"{Guid.NewGuid()}{ext}";
                        var fullPath = Path.Combine(uploadsRoot, fn);
                        using var fs = System.IO.File.Create(fullPath);
                        await f.CopyToAsync(fs);

                        _context.PartnerDocuments.Add(new PartnerDocument
                        {
                            UserId = newUser.Id,
                            PartnerProfileId = profile.Id,
                            // Khớp path với thư mục partner_docs
                            FilePath = $"/uploads/partner_docs/{userFolder}/{fn}"
                        });
                    }
                }
                await _context.SaveChangesAsync();

            }

            // Gán đúng role đã chọn
            var roleResult = await _userManager.AddToRoleAsync(newUser, newUser.Role);
            if (!roleResult.Succeeded)
            {
                TempData["Error"] = "User created, but failed to assign role.";
                return View(registerVM);
            }

            if (registerVM.ApplyAsPartner)
            {
                // ─── GỬI MAIL XÁC NHẬN PARTNER ─────────────────────────
                var subject = "Đơn đăng ký Partner đã được gửi";
                var body = $@"
                    <p>Xin chào <strong>{registerVM.FullName}</strong>,</p>
                    <p>Bạn đã gửi đơn đăng ký Partner thành công. Admin sẽ xét duyệt trong vòng 24 giờ tới.</p>
                    <hr/>
                    <p style='font-size:0.9em;color:#666;'>— NihongoSekai Team</p>
                ";
                await _emailSender.SendEmailAsync(
                    registerVM.EmailAddress!,
                    subject,
                    body
                );
                // ────────────────────────────────────────────────────────

                TempData["PostRegisterMessage"] =
                    "Bạn đã gửi đơn đăng ký thành công. Chúng tôi sẽ xem xét trong vòng 3 ngày.";
                return View("RequestPending");
            }

            if (registerVM.ApplyAsPartner)
            {
                // instead of email confirmation:
                TempData["PostRegisterMessage"] =
                    "Bạn đã gửi đơn đăng ký thành công. Chúng tôi sẽ xem xét trong vòng 3 ngày.";
                return View("RequestPending");
            }
            else
            {
                // existing Learner flow
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                                                  new { userId = newUser.Id, token }, Request.Scheme);
                await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email",
                    $"Please confirm by <a href='{confirmationLink}'>clicking here</a>.");
                TempData["PostRegisterMessage"] = "Account created! Check your email to confirm.";
                return RedirectToAction(nameof(RegisterCompleted));
            }
        }

        // Forgot Password
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Email is null) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email)!;
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                TempData["Error"] = "Email không tồn tại.";
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email! }, Request.Scheme)!;

            await _emailSender.SendEmailAsync(model.Email!, "Reset your password", $"Click here to reset your password: <a href='{resetLink}'>Reset</a>");

            return RedirectToAction("ForgotPasswordConfirmation", "Account");

            //return RedirectToAction("Index", "Loading", new { returnUrl = "Account/ForgotPasswordConfirmation" });
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordVM { Token = token, Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Error"] = "Người dùng không tồn tại.";
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet] //ResetPasswordConfirmation
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // GOOGLE LOGIN
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                // Google login bị hủy → quay về trang login
                TempData["ErrorMessage"] = "Đăng nhập Google thất bại: " + remoteError;
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);

            if (signInResult.Succeeded)
                return LocalRedirect(returnUrl);

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    // ✅ Kiểm tra bị ban
                    if (user.IsBanned)
                    {
                        TempData["Error"] = "Tài khoản của bạn đã bị khóa bởi quản trị viên.";
                        return RedirectToAction("Login");
                    }

                    // Đăng nhập nếu hợp lệ
                    await _signInManager.SignInAsync(user, false);
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    // Tạo user mới
                    user = new ApplicationUser
                    {
                        Email = email,
                        UserName = email,
                        EmailConfirmed = true
                    };
                    await _userManager.CreateAsync(user);
                    await _userManager.AddLoginAsync(user, info);

                    await _signInManager.SignInAsync(user, false);
                    return LocalRedirect(returnUrl);
                }
            }

            TempData["Error"] = "Chưa nhận được email xác minh.";
            return RedirectToAction("Index", "Loading", new { returnUrl = "/Account/Login" });
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? userId = null, string? token = null)
        {
            // 1) Nếu có userId + token => thực hiện confirm
            if (!string.IsNullOrEmpty(userId)
             && !string.IsNullOrEmpty(token))
            {
                var u = await _userManager.FindByIdAsync(userId);
                if (u != null && !u.EmailConfirmed)
                {
                    var r = await _userManager.ConfirmEmailAsync(u, token);
                    if (r.Succeeded)
                        TempData["Success"] = "Xác nhận tài khoản thành công! Bạn đã có thể đăng nhập.";
                    else
                        TempData["Error"] = "Xác nhận tài khoản thất bại. Vui lòng liên hệ hỗ trợ.";
                }
                // 2) Redirect để xóa query-string và trigger lần GET mới
                return RedirectToAction(nameof(Login));
            }

            // 3) Lần GET cuối cùng trả view với model rỗng
            return View(new LoginVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress!);
            if (user == null)
            {
                TempData["Error"] = "Email không tồn tại trên hệ thống. Vui lòng thử lại Email khác.";
                return View(loginVM);
            }

            if (user.Role == "Partner" && !user.IsApproved)
            {
                TempData["Error"] = "Đơn đăng ký làm đối tác hiện vẫn đang chờ phê duyệt.";
                return RedirectToAction("Login");
            }

            // Check if user is banned

            if (user.IsBanned)
            {
                TempData["Error"] = "Tài khoản này đã bị cấm hoạt động. Vui lòng liên hệ hỗ trợ.";
                return View(loginVM);
            }

            // Kiểm tra xác thực email trước
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData["Error"] = "Bạn cần phải xác minh email trước khi đăng nhập.";
                return View(loginVM);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginVM.Password!);
            if (!passwordValid)
            {
                TempData["Error"] = "Mật khẩu không đúng. Vui lòng thử lại.";
                return View(loginVM);
            }

            // Nếu xác thực email OK, đăng nhập
            var signInResult = await _signInManager.PasswordSignInAsync(
                user, loginVM.Password!, isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                TempData["Error"] = "Đăng nhập thất bại. Vui lòng kiểm tra lại định danh của bạn.";
                return View(loginVM);
            }

            // Redirect theo vai trò
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Loading", new { returnUrl = "/Admin" });

            if (await _userManager.IsInRoleAsync(user, "Partner"))
                return RedirectToAction("Index", "Loading", new { returnUrl = "/Partner" });

            return RedirectToAction("Index", "Loading", new { returnUrl = "/Learner" });
        }

        // Xác nhận email
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return View("ConfirmEmail");

            return View("Error");
        }
        //Profile của người dùng
        [Authorize(Roles = "Learner,Partner,Admin")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            // load user + partner‐profile + docs
            var user = await _context.Users
                .Include(u => u.PartnerProfile)
                    .ThenInclude(p => p.Documents)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            // if they’re a Partner, send them to Views/Partners/Profile.cshtml
            if (await _userManager.IsInRoleAsync(user, "Partner"))
                return RedirectToAction("Profile", "Partner");

            // otherwise your Learner flow
            return View("~/Views/Learner/Profile.cshtml", user);
        }

        //Ban feature
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> ToggleBan(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.IsBanned = !user.IsBanned;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Users));
        }

        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("CartId"); //remove cart session after logged out

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Loading", new { returnUrl = "/Courses" });
        }

        // Xem danh sách users (ví dụ admin)
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // Khi truy cập denied
        public IActionResult AccessDenied(string returnUrl) => View();

        [HttpGet]
        public IActionResult LoginFailed(string? error)
        {
            TempData["Error"] = error ?? "Đã có lỗi xảy ra trong quá trình đăng nhập Google.";
            return RedirectToAction("Login");
        }
        [Authorize(Roles = "Partner,Admin")]
        public async Task<IActionResult> ViewProfile(string learnerId, int classroomId)
        {
            if (string.IsNullOrEmpty(learnerId)) return NotFound();

            var learner = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == learnerId && u.Role == "Learner");

            if (learner == null) return NotFound();

            // ✅ Logic kiểm tra quyền sở hữu lớp học (chỉ Partner owner mới được xem)
            if (User.IsInRole("Partner"))
            {
                var isOwner = await _context.ClassroomInstances
                    .AnyAsync(ci => ci.Template.PartnerId == _userManager.GetUserId(User)
                        && ci.Enrollments.Any(e => e.LearnerId == learnerId));

                if (!isOwner) return Forbid();
            }
            // Gán classroomId để nút Back sử dụng
            ViewBag.ClassroomId = classroomId;

            return View("~/Views/Learner/ViewProfile.cshtml", learner);
        }
    }
}

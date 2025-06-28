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

            if (await _userManager.FindByEmailAsync(registerVM.EmailAddress) != null)
            {
                TempData["Error"] = "This email address is already in use.";
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

            // Upload partner file
            if (registerVM.ApplyAsPartner && registerVM.PartnerDocument != null)
            {
                var allowedTypes = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                var ext = Path.GetExtension(registerVM.PartnerDocument.FileName).ToLower();
                if (!allowedTypes.Contains(ext) || registerVM.PartnerDocument.Length > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("PartnerDocument", "Invalid file. Only PDF/JPG/PNG under 10MB allowed.");
                    return View(registerVM);
                }

                string fileName = $"{Guid.NewGuid()}{ext}";
                string uploadPath = Path.Combine(_env.WebRootPath, "uploads", "partner_docs");
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                using var stream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create);
                await registerVM.PartnerDocument.CopyToAsync(stream);

                newUser.PartnerDocumentPath = $"/uploads/partner_docs/{fileName}";
                await _userManager.UpdateAsync(newUser);
            }

            // Gán đúng role đã chọn
            var roleResult = await _userManager.AddToRoleAsync(newUser, newUser.Role);
            if (!roleResult.Succeeded)
            {
                TempData["Error"] = "User created, but failed to assign role.";
                return View(registerVM);
            }

            // Tạo token xác thực email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { userId = newUser.Id, token }, Request.Scheme);

            await _emailSender.SendEmailAsync(newUser.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

            //return RedirectToAction("Index", "Loading", new { returnUrl = "/Account/RegisterCompleted" });
            return RedirectToAction("RegisterCompleted", "Account");

        }


        // Forgot Password
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                TempData["Error"] = "Email isn't exists.";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(model.Email, "Reset your password", $"Click here to reset your password: <a href='{resetLink}'>Reset</a>");

            return RedirectToAction("ForgotPasswordConfirmation", "Account");

            //return RedirectToAction("Index", "Loading", new { returnUrl = "Account/ForgotPasswordConfirmation" });
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordVM { Token = token, Email = email });
        }

 

        // GOOGLE LOGIN
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                TempData["Error"] = $"Error from external provider: {remoteError}";
                return RedirectToAction("Login");
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
                        TempData["Error"] = "Your account has been banned by an administrator.";
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

            TempData["Error"] = "Email claim not received.";
            return RedirectToAction("Index", "Loading", new { returnUrl = "/Account/Login" });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Trả view lần đầu, không truyền model, không có lỗi
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user == null)
            {
                TempData["Error"] = "The email does not exist in the system. Please, try another email!";
                return View(loginVM);
            }

            // Check if user is banned

            if (user.IsBanned)
            {
                TempData["Error"] = "This account has been banned. Please contact support.";
                return View(loginVM);
            }

            // Kiểm tra xác thực email trước
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData["Error"] = "You must confirm your email before logging in.";
                return View(loginVM);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginVM.Password);
            if (!passwordValid)
            {
                TempData["Error"] = "There's something wrong with your password. Try again!";
                return View(loginVM);
            }

            // Nếu xác thực email OK, đăng nhập
            var signInResult = await _signInManager.PasswordSignInAsync(
                user, loginVM.Password, isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                TempData["Error"] = "Login failed. Please, check your credentials and try again.";
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
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
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
    }
}

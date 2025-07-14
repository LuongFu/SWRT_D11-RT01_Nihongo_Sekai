using System;
using System.IO;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using JapaneseLearningPlatform.Models.Partner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Controllers
{
    public class PartnerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ICoursesService _coursesService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PartnerController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env, ICoursesService coursesService, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
            _coursesService = coursesService;
            _signInManager = signInManager;
        }

        // ✅ Default landing for /Partner
        [HttpGet]
        public IActionResult Index() =>
            RedirectToAction(nameof(Profile));

        // 👤 Partner Profile – now includes Documents
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users
                .Include(u => u.PartnerProfile)
                    .ThenInclude(p => p.Documents)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();
            return View("~/Views/Partners/Profile.cshtml", user);
        }

        // ✏️ Edit Profile
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View("~/Views/Partners/EditProfile.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser updatedUser)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FullName = updatedUser.FullName;
            // …update any other fields you expose…
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Profile));
        }

        // 🔒 Reset Password
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public IActionResult ResetPassword() =>
            View("~/Views/Partners/ResetPassword.cshtml");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ChangePasswordError"] = "Vui lòng kiểm tra lại thông tin.";
                return View("~/Views/Partners/ResetPassword.cshtml", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ChangePasswordError"] = "Không tìm thấy tài khoản.";
                return RedirectToAction("ChangePassword");
            }

            var checkOldPassword = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!checkOldPassword)
            {
                TempData["ChangePasswordError"] = "Mật khẩu hiện tại sai.";
                return RedirectToAction("ChangePassword");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                TempData["ChangePasswordError"] = string.Join(" ", result.Errors.Select(e => e.Description));
                return RedirectToAction("ChangePassword");
            }

            TempData["PasswordChangeSuccess"] = "Thay đổi mật khẩu thành công.";
            TempData["ShouldRedirectToLogin"] = true;
            await _signInManager.SignOutAsync();

            return View("~/Views/Partners/ResetPassword.cshtml");
        }

        // 📷 Upload Profile Picture
        [Authorize(Roles = "Partner")]
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || profilePicture == null || profilePicture.Length == 0)
                return RedirectToAction(nameof(Profile));

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profile");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(profilePicture.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await profilePicture.CopyToAsync(stream);

            user.ProfilePicturePath = $"/uploads/profile/{fileName}";
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Profile));
        }

        // 📄 Upload a new Partner Document
        [Authorize(Roles = "Partner")]
        [HttpPost]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file to upload.");
                return RedirectToAction(nameof(Profile));
            }

            var userId = _userManager.GetUserId(User);
            var profile = await _context.PartnerProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return BadRequest();

            var webRoot = _env.WebRootPath
                  ?? throw new InvalidOperationException("WebRootPath not set");
            var uploadsRoot = Path.Combine(webRoot, "uploads", "partner_docs", userId);
            Directory.CreateDirectory(uploadsRoot);

            var ext = Path.GetExtension(file.FileName);
            var fname = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadsRoot, fname);

            using var fs = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fs);

            var doc = new PartnerDocument
            {
                UserId = userId,
                PartnerProfileId = profile.Id,
                FilePath = $"/uploads/partners/{userId}/{fname}"
            };
            _context.PartnerDocuments.Add(doc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Profile));
        }

        // 🗑 Remove an existing Partner Document
        [Authorize(Roles = "Partner")]
        [HttpPost]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var doc = await _context.PartnerDocuments.FindAsync(id);
            if (doc == null || doc.UserId != _userManager.GetUserId(User))
                return Forbid();

            // optional: delete the file itself
            var physicalPath = Path.Combine(_env.WebRootPath,
                doc.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(physicalPath))
                System.IO.File.Delete(physicalPath);

            _context.PartnerDocuments.Remove(doc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Profile));
        }

        // Word of the day
        public DbSet<DailyWord> DailyWords { get; set; }
    }
}

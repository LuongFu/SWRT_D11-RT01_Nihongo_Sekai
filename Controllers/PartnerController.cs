using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Controllers
{
    public class PartnerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public PartnerController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // 👤 Trang hồ sơ đối tác
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View("~/Views/Partners/Profile.cshtml", user);
        }

        // ✏️ Sửa hồ sơ
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View("~/Views/Partners/EditProfile.cshtml", user);
        }

        [Authorize(Roles = "Partner")]
        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser updatedUser)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FullName = updatedUser.FullName;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Profile");
        }

        // 🔒 Đặt lại mật khẩu
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View("~/Views/Partners/ResetPassword.cshtml");
        }

        [Authorize(Roles = "Partner")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Partners/ResetPassword.cshtml", model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View("~/Views/Partners/ResetPassword.cshtml", model);
            }

            return RedirectToAction("Profile");
        }

        // 📷 Tải lên ảnh đại diện
        [Authorize(Roles = "Partner")]
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || profilePicture == null || profilePicture.Length == 0)
                return RedirectToAction("Profile");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profile");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(profilePicture.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            user.ProfilePicturePath = $"/uploads/profile/{fileName}";
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Profile");
        }

        // ✅ Trang mặc định khi truy cập /Partner
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Profile");
        }
    }
}

using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Services;
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

        // ✅ Trang mặc định khi truy cập /Partner
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Profile");
        }

        // 👤 Trang hồ sơ đối tác
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View("~/Views/Partner/Profile.cshtml", user);
        }

        // ✏️ Sửa hồ sơ
        [Authorize(Roles = "Partner")]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View("~/Views/Partner/EditProfile.cshtml", user);
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
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ChangePasswordError"] = "Vui lòng kiểm tra lại thông tin.";
                return RedirectToAction("ChangePassword");
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

            // ✅ Thành công → hiển thị thông báo và chờ redirect
            TempData["PasswordChangeSuccess"] = "Thay đổi mật khẩu thành công.";
            TempData["ShouldRedirectToLogin"] = true;
            await _signInManager.SignOutAsync();

            return View(); // đảm bảo luôn có return
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
    }
}

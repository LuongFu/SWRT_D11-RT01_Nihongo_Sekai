using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JapaneseLearningPlatform.Controllers
{
    public class LearnerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public LearnerController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env; 
        }

        public ActionResult Index() => View();

        public ActionResult Details(int id) => View();

        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try { return RedirectToAction(nameof(Index)); }
            catch { return View(); }
        }

        public ActionResult Edit(int id) => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try { return RedirectToAction(nameof(Index)); }
            catch { return View(); }
        }

        public ActionResult Delete(int id) => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try { return RedirectToAction(nameof(Index)); }
            catch { return View(); }
        }

        // 🧾 View danh sách khóa học đã mua
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> MyPurchasedCourses(int page = 1, int pageSize = 6)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var purchasedCourseIds = await _context.Orders
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.OrderItems.Select(oi => oi.CourseId))
                .Distinct()
                .ToListAsync();

            var allPurchasedCourses = await _context.Courses
                .Where(c => purchasedCourseIds.Contains(c.Id))
                .ToListAsync();

            var totalItems = allPurchasedCourses.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedCourses = allPurchasedCourses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View("MyPurchasedCourses", pagedCourses);
        }

        // 👤 Trang hồ sơ người học
        [Authorize(Roles = "Learner")]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View("~/Views/Learner/Profile.cshtml", user);
        }

        // ✏️ Sửa thông tin hồ sơ
        [Authorize(Roles = "Learner")]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View("~/Views/Learner/EditProfile.cshtml", user);
        }
        //Đổi mật khẩu
        [Authorize(Roles = "Learner")]
        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser updatedUser)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FullName = updatedUser.FullName;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Profile");
        }

        [Authorize(Roles = "Learner")]
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View("~/Views/Learner/ResetPassword.cshtml");
        }

        [Authorize(Roles = "Learner")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View("~/Views/Learner/ResetPassword.cshtml", model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View("~/Views/Learner/ResetPassword.cshtml", model);
            }

            return RedirectToAction("Profile");
        }
        // 📷 Tải lên ảnh đại diện
        [Authorize(Roles = "Learner")]
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || profilePicture == null || profilePicture.Length == 0)
                return RedirectToAction("Profile");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profile");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(profilePicture.FileName)}";
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

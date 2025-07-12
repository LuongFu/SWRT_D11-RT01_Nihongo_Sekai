using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.Static;
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
        private readonly ICoursesService _coursesService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LearnerController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env, ICoursesService coursesService, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
            _coursesService = coursesService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.IsAuthenticated
                ? _userManager.GetUserId(User)
                : null;

            var cartId = HttpContext.Session.GetString("CartId") ?? Guid.NewGuid().ToString();
            HttpContext.Session.SetString("CartId", cartId);

            var courses = await _coursesService.GetAllCoursesWithPurchaseInfoAsync(userId, cartId);

            // chỉ lấy 3 khóa học rẻ nhất làm Featured
            var featuredCourses = courses.OrderBy(c => c.FinalPrice).Take(3).ToList();

            return View(featuredCourses); // Model sẽ là List<CourseWithPurchaseVM>
        }


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
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View("~/Views/Learner/Profile.cshtml", user);
        }

        // ✏️ Sửa thông tin hồ sơ
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

        // 🔐 Đổi mật khẩu Learner
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

            // ❌ Nếu mật khẩu mới trùng mật khẩu hiện tại
            if (model.CurrentPassword == model.NewPassword)
            {
                TempData["ChangePasswordError"] = "New password must not be the same as the current password.";
                return RedirectToAction("ChangePassword");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                TempData["ChangePasswordError"] = string.Join(" ", result.Errors.Select(e => e.Description));
                return RedirectToAction("ChangePassword");
            }

            // ✅ Thành công → sign out và cho phép redirect sau vài giây
            TempData["PasswordChangeSuccess"] = "Thay đổi mật khẩu thành công. Bạn sẽ được chuyển hướng đến trang đăng nhập...";
            TempData["ShouldRedirectToLogin"] = true;
            await _signInManager.SignOutAsync();

            return View();
        }



        // 📷 Tải lên ảnh đại diện
        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            if (profilePicture == null || profilePicture.Length == 0)
            {
                TempData["UploadError"] = "Please select a photo before uploading.";
                return RedirectToAction("Profile");
            }

            var extension = Path.GetExtension(profilePicture.FileName)?.ToLower();
            if (string.IsNullOrWhiteSpace(extension))
            {
                TempData["UploadError"] = "Invalid file. No file extension found.";
                return RedirectToAction("Profile");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowedExtensions.Contains(extension))
            {
                TempData["UploadError"] = "Invalid file type. Please select an image with one of the following formats: .jpg, .jpeg, .png, .gif, .webp.";
                return RedirectToAction("Profile");
            }

            // 📁 Tạo thư mục nếu chưa có
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profile");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // 🧹 Xoá ảnh cũ nếu có (trừ khi đang dùng default)
            if (!string.IsNullOrEmpty(user.ProfilePicturePath) && !user.ProfilePicturePath.Contains("default-img.jpg"))
            {
                var oldPath = Path.Combine(_env.WebRootPath, user.ProfilePicturePath.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            // 📥 Lưu ảnh mới
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            user.ProfilePicturePath = $"/uploads/profile/{fileName}";
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Profile");
        }



        // 📚 Lớp học của tôi (hiển thị thời khóa biểu và partner)
        [HttpGet]
        public async Task<IActionResult> MyClassroom()
        {
            return View("~/Views/Learner/MyClassroom.cshtml");
        }

        [Authorize(Roles = UserRoles.Learner)]
        public async Task<IActionResult> MyEnrolledClassrooms()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var enrollments = await _context.ClassroomEnrollments
                .Where(e => e.LearnerId == userId && e.IsPaid && !e.HasLeft)
                .Include(e => e.Instance)
                    .ThenInclude(i => i.Template)
                .ToListAsync();

            var model = enrollments.Select(e => new EnrolledClassroomVM
            {
                EnrollmentId = e.Id,
                InstanceId = e.InstanceId,
                ClassTitle = e.Instance?.Template?.Title ?? "(Unknown)",
                StartDate = e.Instance.StartDate,
                HasLeft = e.HasLeft
            }).ToList();

            return View(model);
        }

    }
}

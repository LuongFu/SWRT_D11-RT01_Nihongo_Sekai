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
            var userId = _userManager.GetUserId(User); 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            return View("~/Views/Learner/Profile.cshtml", user);
        }

        // ✏️ Sửa thông tin hồ sơ
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = _userManager.GetUserId(User); // ✅ Lấy userId từ Identity
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId); // ✅ Lấy fresh từ DB

            if (user == null) return NotFound();

            var vm = new EditProfileVM
            {
                FullName = user.FullName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Description = user.Description,
                JobName = user.JobName,
                Facebook = user.Facebook,
                YouTube = user.YouTube,
                ProfilePicturePath = user.ProfilePicturePath,
                ShowDeleteButton = !string.IsNullOrEmpty(user.ProfilePicturePath)
                    && !user.ProfilePicturePath.Contains("default-img.jpg")
            };

            return View("~/Views/Learner/EditProfile.cshtml", vm);
        }

        [Authorize(Roles = "Learner")]
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                model.ProfilePicturePath = (await _userManager.GetUserAsync(User))?.ProfilePicturePath;
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Address = model.Address;
            user.BirthDate = model.BirthDate;
            user.Gender = model.Gender;
            user.Description = model.Description;
            user.JobName = model.JobName;
            user.Facebook = model.Facebook;
            user.YouTube = model.YouTube;

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                var phoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!phoneResult.Succeeded)
                {
                    ModelState.AddModelError("PhoneNumber", "Số điện thoại không hợp lệ.");
                    model.ProfilePicturePath = user.ProfilePicturePath;
                    return View(model);
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                model.ProfilePicturePath = user.ProfilePicturePath;
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
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
                TempData["ChangePasswordError"] = "Mật khẩu mới phải trùng với mật khẩu cũ.";
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
            if (user == null) return NotFound();

            if (profilePicture == null || profilePicture.Length == 0)
            {
                TempData["UploadError"] = "Vui lòng chọn ảnh trước khi tải lên.";
                return RedirectToAction("Profile");
            }

            var extension = Path.GetExtension(profilePicture.FileName)?.ToLower();
            if (string.IsNullOrWhiteSpace(extension))
            {
                TempData["UploadError"] = "File không hợp lệ. Không tìm thấy phần mở rộng.";
                return RedirectToAction("Profile");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            if (!allowedExtensions.Contains(extension))
            {
                TempData["UploadError"] = "File không hợp lệ. Vui lòng chọn ảnh thuộc một trong các định dạng: .jpg, .jpeg, .png, .gif, .webp.";
                return RedirectToAction("Profile");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profile");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            if (!string.IsNullOrEmpty(user.ProfilePicturePath) && !user.ProfilePicturePath.Contains("default-img.jpg"))
            {
                var oldPath = Path.Combine(_env.WebRootPath, user.ProfilePicturePath.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            user.ProfilePicturePath = $"/uploads/profile/{fileName}";
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["UploadError"] = string.Join(", ", result.Errors.Select(e => e.Description));
                return RedirectToAction("Profile");
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Ảnh đại diện đã được cập nhật!";
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
                HasLeft = e.HasLeft,
                ThumbnailUrl = e.Instance?.Template?.ImageURL // ánh xạ ảnh đại diện
            }).ToList();

            return View(model);
        }

    }
}

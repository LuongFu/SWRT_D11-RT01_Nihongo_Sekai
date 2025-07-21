using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Cart;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace JapaneseLearningPlatform.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICoursesService _service;
        private readonly ShoppingCart _shoppingCart;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrdersService _orderService;
        private readonly ICoursesService _courseService;
        //private readonly ICertificateService _certService;
        //private readonly IEmailSender _emailSender;
        private readonly ICourseRatingService _ratingService;
        public CoursesController(ICoursesService service, ShoppingCart shoppingCart, AppDbContext context, IHttpContextAccessor httpContextAccessor, IOrdersService orderService, ICoursesService courseService, ICourseRatingService ratingService)
        {
            _service = service;
            _shoppingCart = shoppingCart;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
            _courseService = courseService;
            //_certService = certService;     // gán
            //_emailSender = emailSender;
            _ratingService = ratingService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _shoppingCart.GetShoppingCartItems().Select(c => c.Course.Id).ToList();

            var purchasedCourseIds = _context.Orders
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.OrderItems.Select(oi => oi.CourseId))
                .ToHashSet();

            var courses = await _context.Courses
                .Include(c => c.Sections)
                    .ThenInclude(s => s.ContentItems)
                .ToListAsync();

            var totalItems = courses.Count;
            var itemsToDisplay = courses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Lấy progress cho tất cả course của user
            var completedContent = await _context.CourseContentProgresses
                .Where(p => p.UserId == userId && p.IsCompleted)
                .ToListAsync();

            var viewModel = itemsToDisplay.Select(course =>
            {
                double progress = 0;
                if (purchasedCourseIds.Contains(course.Id))
                {
                    int totalContentItems = course.Sections.Sum(s => s.ContentItems.Count);
                    int completedItems = completedContent.Count(c => c.CourseId == course.Id);
                    progress = totalContentItems > 0 ? (completedItems / (double)totalContentItems) * 100 : 0;
                }

                return new CourseWithPurchaseVM
                {
                    Course = course,
                    IsInCart = cartItems.Contains(course.Id),
                    IsPurchased = purchasedCourseIds.Contains(course.Id),
                    ProgressPercent = progress
                };
            }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(viewModel);
        }




        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Filter(string searchString, CourseCategory? selectedCategory, int? minPrice, int page = 1, int pageSize = 6)
        {
            // B1: Lấy tất cả khóa học và đưa về List để tránh lỗi delegate inference
            var allCourses = await _service.GetAllAsync();
            var allCoursesList = allCourses.ToList();

            // B2: Lọc theo từ khóa
            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearch = searchString.ToLower();
                allCoursesList = allCoursesList
                    .Where(c =>
                        (!string.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(lowerSearch)) ||
                        (!string.IsNullOrEmpty(c.Description) && c.Description.ToLower().Contains(lowerSearch)))
                    .ToList();
            }

            // B3: Lọc theo category nếu có chọn
            if (selectedCategory != null)
            {
                allCoursesList = allCoursesList
                    .Where(c => c.CourseCategory == selectedCategory)
                    .ToList();
            }
            // === B3.1: Chuyển minPrice thành maxPrice tương ứng ===
            int? maxPrice = null;
            if (minPrice.HasValue)
            {
                switch (minPrice.Value)
                {
                    case 0: maxPrice = 100_000; break;   // Dưới 100k
                    case 101_000: maxPrice = 500_000; break;   // 101k–500k
                    case 501_000: maxPrice = 1_000_000; break;   // 501k–1M
                    case 1_000_001: maxPrice = null; break;   // Trên 1M
                    default:
                        minPrice = null; maxPrice = null;
                        break;
                }
            }

            // B3.2: Áp filter giá
            if (minPrice.HasValue)
                allCoursesList = allCoursesList
                    .Where(c => c.FinalPrice >= minPrice.Value)
                    .ToList();
            if (maxPrice.HasValue)
                allCoursesList = allCoursesList
                    .Where(c => c.FinalPrice <= maxPrice.Value)
                    .ToList();

            // B4: Phân trang
            var totalItems = allCoursesList.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagedCourses = allCoursesList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // B5: Lấy thông tin người dùng và giỏ hàng
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var cartCourseIds = _shoppingCart.GetShoppingCartItems()
                .Where(i => i.Course != null)
                .Select(i => i.Course.Id)
                .ToList();

            var purchasedCourseIds = !string.IsNullOrEmpty(userId)
                ? await _orderService.GetPurchasedCourseIdsByUser(userId)
                : new List<int>();

            // B6: Map sang ViewModel
            var viewModelList = pagedCourses.Select(course => new CourseWithPurchaseVM
            {
                Course = course,
                IsPurchased = purchasedCourseIds.Contains(course.Id),
                IsInCart = cartCourseIds.Contains(course.Id)
            }).ToList();

            // B7: Truyền dữ liệu cho View
            ViewBag.MinPrice = minPrice;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.SelectedCategory = selectedCategory;
            ViewBag.ShowAdvancedFilter = true;

            return View("Index", viewModelList);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, int? videoId = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartId = _shoppingCart.ShoppingCartId;

            // 1) Lấy toàn bộ cấu trúc course + tiến độ + quiz…
            var vm = await _service.GetCourseHierarchyAsync(id, userId, cartId);
            if (vm == null || vm.Course == null)
                return View("NotFound");

            // ─────── BỔ SUNG PHẦN RATING ───────

            // 2) Thống kê số sao trung bình, tổng đánh giá và breakdown
            var (avg, total, counts) = await _ratingService.GetStatsAsync(id);
            vm.AverageRating = avg;
            vm.TotalRatings = total;
            vm.RatingCounts = counts;

            // 3) Lấy 5 bình luận mới nhất (đã include luôn thông tin User)
            vm.LatestRatings = (await _ratingService
                                        .GetLatestAsync(id, pageSize: 5, page: 1, sort: "Newest"))
                                        .ToList();

            // ────────────────────────────────────

            return View(vm);
        }



        //GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            var courseDropdownsData = await _service.GetNewCourseDropdownsValues();
            ViewBag.Videos = new SelectList(courseDropdownsData.Videos, "Id", "VideoDescription");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewCourseVM course)
        {
            if (!ModelState.IsValid)
            {
                var courseDropdownsData = await _service.GetNewCourseDropdownsValues();
                ViewBag.Videos = new SelectList(courseDropdownsData.Videos, "Id", "VideoDescription");

                return View(course);
            }

            if (course.ImageFile != null)
            {
                course.ImageURL = await _courseService.SaveFileAsync(course.ImageFile, "uploads/courses");
            }

            await _service.AddNewCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }


        //GET: Courses/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            //var courseDetails = await _service.GetCourseByIdAsync(id);
            var courseDetails = await _context.Courses
        .FirstOrDefaultAsync(c => c.Id == id);
            if (courseDetails == null) return View("NotFound");

            var response = new NewCourseVM()
            {
                Id = courseDetails.Id,
                Name = courseDetails.Name,
                Description = courseDetails.Description,
                Price = courseDetails.Price,
                DiscountPercent = courseDetails?.DiscountPercent,
                StartDate = (DateTime)courseDetails?.StartDate,
                EndDate = (DateTime)courseDetails?.EndDate,
                ImageURL = courseDetails.ImageURL,
                CourseCategory = courseDetails.CourseCategory
            };

            var courseDropdownsData = await _service.GetNewCourseDropdownsValues();
            ViewBag.Videos = new SelectList(courseDropdownsData.Videos, "Id", "VideoDescription");


            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewCourseVM course)
        {
            if (id != course.Id) return View("NotFound");

            if (course.ImageFile != null)
            {
                course.ImageURL = await _courseService.SaveFileAsync(course.ImageFile, "uploads/courses");
            }
            if (!ModelState.IsValid)
            {
                var courseDropdownsData = await _service.GetNewCourseDropdownsValues();

                ViewBag.Videos = new SelectList(courseDropdownsData.Videos, "Id", "VideoDescription");

                return View(course);
            }

            await _service.UpdateCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleContentCompletion([FromBody] ToggleCompletionVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false });

            // Tính progress hiện tại
            int totalItems = await _context.CourseContentItems
                .CountAsync(ci => ci.Section.CourseId == model.CourseId);

            int completedCountBefore = await _context.CourseContentProgresses
                .CountAsync(p => p.UserId == userId && p.CourseId == model.CourseId && p.IsCompleted);

            double currentProgress = totalItems > 0 ? (completedCountBefore / (double)totalItems) * 100 : 0;

            // Nếu đã đạt 100% thì bỏ qua việc uncheck
            if (currentProgress >= 100 && !model.IsCompleted)
            {
                return Json(new { success = false, progress = 100 });
            }

            // Tìm content progress hiện có
            var existing = await _context.CourseContentProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId &&
                                          p.CourseId == model.CourseId &&
                                          p.ContentItemId == model.ContentItemId);

            if (existing == null)
            {
                existing = new CourseContentProgress
                {
                    UserId = userId,
                    CourseId = model.CourseId,
                    ContentItemId = model.ContentItemId,
                    IsCompleted = model.IsCompleted,
                    CompletedAt = DateTime.UtcNow
                };
                _context.CourseContentProgresses.Add(existing);
            }
            else
            {
                existing.IsCompleted = model.IsCompleted;
                existing.CompletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            // Tính progress % mới
            int completedCount = await _context.CourseContentProgresses
                .CountAsync(p => p.UserId == userId && p.CourseId == model.CourseId && p.IsCompleted);

            double progress = totalItems > 0 ? (completedCount / (double)totalItems) * 100 : 0;

            return Json(new { success = true, progress });
        }



        [HttpGet]
        public async Task<IActionResult> GetProgress(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            int totalItems = await _context.CourseContentItems
                .Where(ci => ci.Section.CourseId == id)
                .CountAsync();

            int completedItems = await _context.CourseContentProgresses
                .Where(p => p.CourseId == id && p.UserId == userId && p.IsCompleted)
                .CountAsync();

            double progress = totalItems > 0 ? (completedItems / (double)totalItems) * 100 : 0;
            if (progress > 100) progress = 100; // chặn vượt quá 100%

            return Json(new { progress = progress.ToString("0") });
        }
    }
}
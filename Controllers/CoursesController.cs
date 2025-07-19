using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Cart;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Claims;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CoursesController : Controller
    {
        private readonly ICoursesService _service;
        private readonly ShoppingCart _shoppingCart;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrdersService _orderService;
        private readonly ICoursesService _courseService;
        public CoursesController(ICoursesService service, ShoppingCart shoppingCart, AppDbContext context, IHttpContextAccessor httpContextAccessor, IOrdersService orderService, ICoursesService courseService)
        {
            _service = service;
            _shoppingCart = shoppingCart;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
            _courseService = courseService;
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
                .ToListAsync();

            var totalItems = courses.Count;
            var itemsToDisplay = courses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = itemsToDisplay.Select(course => new CourseWithPurchaseVM
            {
                Course = course,
                IsInCart = cartItems.Contains(course.Id),
                IsPurchased = purchasedCourseIds.Contains(course.Id)
            });

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
            var cartId = _shoppingCart.ShoppingCartId; // fix an toàn dựa trên session
            var courseHierarchy = await _service.GetCourseHierarchyAsync(id, userId, cartId);
            if (courseHierarchy == null || courseHierarchy.Course == null) return View("NotFound");

            return View(courseHierarchy);
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
    }
}
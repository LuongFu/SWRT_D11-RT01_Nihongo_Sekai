using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NihongoSekaiPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Cart;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public CoursesController(ICoursesService service, ShoppingCart shoppingCart, AppDbContext context, IHttpContextAccessor httpContextAccessor, IOrdersService orderService)
        {
            _service = service;
            _shoppingCart = shoppingCart;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
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
                .Include(c => c.Videos_Courses)
                .ThenInclude(a => a.Video)
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
        public async Task<IActionResult> Filter(string searchString, int page = 1, int pageSize = 6)
        {
            // Giữ nguyên kiểu var
            var allCourses = await _service.GetAllAsync();

            // Lọc theo search string
            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearch = searchString.ToLower();
                allCourses = allCourses
                    .Where(c =>
                        (!string.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(lowerSearch)) ||
                        (!string.IsNullOrEmpty(c.Description) && c.Description.ToLower().Contains(lowerSearch)));
            }

            // Chuyển sang list để phân trang
            var filteredCoursesList = allCourses.ToList();
            var totalItems = filteredCoursesList.Count;

            var pagedCourses = filteredCoursesList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // take user and cart information
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cartCourseIds = _shoppingCart
                ?.GetShoppingCartItems()
                ?.Where(i => i.Course != null)
                .Select(i => i.Course.Id)
                .ToList() ?? new List<int>();

            var purchasedCourseIds = new List<int>();
            if (!string.IsNullOrEmpty(userId))
            {
                purchasedCourseIds = await _orderService.GetPurchasedCourseIdsByUser(userId);
            }

            var viewModelList = pagedCourses.Select(course => new CourseWithPurchaseVM
            {
                Course = course,
                IsPurchased = purchasedCourseIds.Contains(course.Id),
                IsInCart = cartCourseIds.Contains(course.Id)
            }).ToList();

            // ViewBag paging
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.SearchString = searchString;

            return View("Index", viewModelList);
        }





        //GET: Courses/Details/...
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Videos_Courses).ThenInclude(vc => vc.Video)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return View("NotFound");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isPurchased = await _context.Orders
                .Include(o => o.OrderItems)
                .AnyAsync(o => o.UserId == userId && o.OrderItems.Any(i => i.CourseId == id));

            var isInCart = _shoppingCart.GetShoppingCartItems().Any(i => i.CourseId == id);

            var videoIds = await _context.Videos_Courses
                .Where(vc => vc.CourseId == id)
                .Select(vc => vc.VideoId)
                .ToListAsync();

            var videos = await _context.Videos
                .Where(v => videoIds.Contains(v.Id))
                .ToListAsync();

            var viewModel = new CourseDetailVM
            {
                Course = course,
                IsPurchased = isPurchased,
                IsInCart = isInCart,
                Videos = videos
            };

            return View(viewModel);
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
        .Include(c => c.Videos_Courses)
        .Include(c => c.Videos_Courses)
        .FirstOrDefaultAsync(c => c.Id == id);
            if (courseDetails == null) return View("NotFound");

            var response = new NewCourseVM()
            {
                Id = courseDetails.Id,
                Name = courseDetails.Name,
                Description = courseDetails.Description,
                Price = courseDetails.Price,
                StartDate = courseDetails.StartDate,
                EndDate = courseDetails.EndDate,
                ImageURL = courseDetails.ImageURL,
                CourseCategory = courseDetails.CourseCategory,
                VideoIds = courseDetails.Videos_Courses.Select(vc => vc.VideoId).ToList()
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
    }
}
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
        //public async Task<IActionResult> Index()
        //{
        //    var allCourses = await _service.GetAllAsync(n => n.Cinema);
        //    return View(allCourses);
        //}
        public async Task<IActionResult> Index()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cartItems = _shoppingCart.GetShoppingCartItems().Select(c => c.Course.Id).ToList();
        var purchasedCourseIds = _context.Orders
            .Where(o => o.UserId == userId)
            .SelectMany(o => o.OrderItems.Select(oi => oi.CourseId))
            .ToHashSet();

        var courses = await _context.Courses.Include(c => c.Actors_Courses).ThenInclude(a => a.Actor).ToListAsync();

        var viewModel = courses.Select(course => new CourseWithPurchaseVM
        {
            Course = course,
            IsInCart = cartItems.Contains(course.Id),
            IsPurchased = purchasedCourseIds.Contains(course.Id)
        });

    return View(viewModel);
}


        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            // Bước 1: Lấy toàn bộ khóa học
            var allCourses = await _service.GetAllAsync();

            // Bước 2: Lọc theo từ khóa (theo tên hoặc mô tả)
            var filteredCourses = allCourses;
            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearch = searchString.ToLower();
                filteredCourses = allCourses
                    .Where(c =>
                        (!string.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(lowerSearch)) ||
                        (!string.IsNullOrEmpty(c.Description) && c.Description.ToLower().Contains(lowerSearch)))
                    .ToList();
            }

            // Bước 3: Lấy ID người dùng hiện tại (có thể null nếu chưa login)
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Bước 4: Lấy danh sách giỏ hàng
            var cartCourseIds = _shoppingCart
                ?.GetShoppingCartItems()
                ?.Where(i => i.Course != null)
                .Select(i => i.Course.Id)
                .ToList() ?? new List<int>();

            // Bước 5: Nếu có user => lấy danh sách khóa học đã mua
            var purchasedCourseIds = new List<int>();
            if (!string.IsNullOrEmpty(userId))
            {
                purchasedCourseIds = await _orderService.GetPurchasedCourseIdsByUser(userId);
            }

            // Bước 6: Map sang ViewModel
            var viewModelList = filteredCourses.Select(course => new CourseWithPurchaseVM
            {
                Course = course,
                IsPurchased = purchasedCourseIds.Contains(course.Id),
                IsInCart = cartCourseIds.Contains(course.Id)
            }).ToList();

            return View("Index", viewModelList);
        }




        //GET: Courses/Details/...
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Cinema)
                .Include(c => c.Producer)
                .Include(c => c.Actors_Courses).ThenInclude(ac => ac.Actor)
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

            ViewBag.Cinemas = new SelectList(courseDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(courseDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(courseDropdownsData.Actors, "Id", "FullName");
            ViewBag.Videos = new SelectList(courseDropdownsData.Videos, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewCourseVM course)
        {
            if (!ModelState.IsValid)
            {
                var courseDropdownsData = await _service.GetNewCourseDropdownsValues();

                ViewBag.Cinemas = new SelectList(courseDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(courseDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(courseDropdownsData.Actors, "Id", "FullName");
                ViewBag.Videos = new SelectList(courseDropdownsData.Videos, "Id", "FullName");

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
        .Include(c => c.Actors_Courses)
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
                CinemaId = courseDetails.CinemaId,
                ProducerId = courseDetails.ProducerId,
                ActorIds = courseDetails.Actors_Courses.Select(ac => ac.ActorId).ToList(),
                VideoIds = courseDetails.Videos_Courses.Select(vc => vc.VideoId).ToList()
                //VideoIds = courseDetails.Videos_Courses?.Select(vc => vc.VideoId).ToList() ?? new List<int>()
            };

            var courseDropdownsData = await _service.GetNewCourseDropdownsValues();
            ViewBag.Cinemas = new SelectList(courseDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(courseDropdownsData.Producers, "Id", "FullName");
            ViewBag.Videos = new SelectList(courseDropdownsData.Videos, "Id", "VideoDescription");
            ViewBag.Actors = new SelectList(courseDropdownsData.Actors, "Id", "FullName");
            

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewCourseVM course)
        {
            if (id != course.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var courseDropdownsData = await _service.GetNewCourseDropdownsValues();

                ViewBag.Cinemas = new SelectList(courseDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(courseDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(courseDropdownsData.Actors, "Id", "FullName");
                ViewBag.Videos = new SelectList(courseDropdownsData.Videos, "Id", "FullName");

                return View(course);
            }

            await _service.UpdateCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }
    }
}
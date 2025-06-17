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
        public CoursesController(ICoursesService service, ShoppingCart shoppingCart, AppDbContext context)
        {
            _service = service;
            _shoppingCart = shoppingCart;
            _context = context;
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
            var allCourses = await _service.GetAllAsync(n => n.Cinema);

            if (!string.IsNullOrEmpty(searchString))
            {
                //var filteredResult = allCourses.Where(n => n.Name.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                var filteredResultNew = allCourses.Where(n => string.Equals(n.Name, searchString, StringComparison.CurrentCultureIgnoreCase) || string.Equals(n.Description, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("Index", filteredResultNew);
            }

            return View("Index", allCourses);
        }

        //GET: Courses/Details/1
        [AllowAnonymous]
        //public async Task<IActionResult> Details(int id)
        //{
        //    var courseDetail = await _service.GetCourseByIdAsync(id);
        //    return View(courseDetail);
        //}
        //public async Task<IActionResult> Details(int id)
        //{
        //    var course = await _context.Courses
        //        .Include(c => c.Cinema)
        //        .Include(c => c.Producer)
        //        .Include(c => c.Actors_Courses).ThenInclude(ac => ac.Actor)
        //        .FirstOrDefaultAsync(c => c.Id == id);

        //    if (course == null) return NotFound();

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var isPurchased = await _context.Orders
        //        .AnyAsync(o => o.UserId == userId && o.OrderItems.Any(oi => oi.CourseId == id));

        //    var isInCart = _shoppingCart.GetShoppingCartItems().Any(item => item.Course.Id == id);

        //    var viewModel = new CourseDetailVM
        //    {
        //        Course = course,
        //        IsPurchased = isPurchased,
        //        IsInCart = isInCart
        //    };

        //    return View(viewModel); // Trả đúng ViewModel như View mong đợi
        //}
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Cinema)
                .Include(c => c.Producer)
                .Include(c => c.Actors_Courses).ThenInclude(ac => ac.Actor)
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
            var movieDropdownsData = await _service.GetNewCourseDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewCourseVM movie)
        {
            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _service.GetNewCourseDropdownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

                return View(movie);
            }

            await _service.AddNewCourseAsync(movie);
            return RedirectToAction(nameof(Index));
        }


        //GET: Courses/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await _service.GetCourseByIdAsync(id);
            if (movieDetails == null) return View("NotFound");

            var response = new NewCourseVM()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate,
                ImageURL = movieDetails.ImageURL,
                CourseCategory = movieDetails.CourseCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                ActorIds = movieDetails.Actors_Courses.Select(n => n.ActorId).ToList(),
            };

            var movieDropdownsData = await _service.GetNewCourseDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewCourseVM movie)
        {
            if (id != movie.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _service.GetNewCourseDropdownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

                return View(movie);
            }

            await _service.UpdateCourseAsync(movie);
            return RedirectToAction(nameof(Index));
        }
    }
}
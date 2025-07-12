using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Cart;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize] 
    public class OrdersController : Controller
    {
        private readonly ICoursesService _coursesService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;
        private readonly AppDbContext _context;

        public OrdersController(ICoursesService coursesService, ShoppingCart shoppingCart, IOrdersService ordersService, AppDbContext context)
        {
            _coursesService = coursesService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }

        //public IActionResult ShoppingCart()
        //{
        //    var items = _shoppingCart.GetShoppingCartItems();
        //    _shoppingCart.ShoppingCartItems = items;

        //    var response = new ShoppingCartVM()
        //    {
        //        ShoppingCart = _shoppingCart,
        //        ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
        //    };

        //    return View(response);
        //}
        public async Task<IActionResult> ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recommendedCourses = await _coursesService.GetRecommendedCoursesAsync(userId);

            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
                RecommendedCourses = (await _coursesService.GetRecommendedCoursesAsync(userId)).ToList()
            };

            return View(response);
        }


        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _coursesService.GetCourseByIdAsync(id);

            if (item != null)
            {
                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _coursesService.GetCourseByIdAsync(id);

            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        //public async Task<IActionResult> CompleteOrder()
        //{
        //    var items = _shoppingCart.GetShoppingCartItems();
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

        //    await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
        //    await _shoppingCart.ClearShoppingCartAsync();

        //    return View("OrderCompleted");
        //}
        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();

            var recommendedCourses = await _coursesService.GetRecommendedCoursesAsync(userId);

            var vm = new ShoppingCartVM
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = 0, // Đã thanh toán xong
                RecommendedCourses = recommendedCourses.ToList()
            };

            return View("OrderCompleted", vm);
        }

    }
}

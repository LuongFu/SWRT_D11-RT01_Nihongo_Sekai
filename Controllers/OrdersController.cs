using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Cart;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize] 
    public class OrdersController : Controller
    {
        private readonly ICoursesService _coursesService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly VNPayService _vnPayService;

        public OrdersController(
            ICoursesService coursesService,
            ShoppingCart shoppingCart,
            IOrdersService ordersService,
            AppDbContext context,
            IConfiguration config,
            VNPayService vnPayService)
        {
            _coursesService = coursesService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
            _context = context;
            _config = config;
            _vnPayService = vnPayService;
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
                ShoppingCartTotal = 0,
                RecommendedCourses = recommendedCourses.ToList()
            };
            return View("OrderCompleted", vm);
        }

        [HttpPost]
        public IActionResult VNPayCheckout()
        {
            // 1. Lấy giỏ hàng hiện tại
            var items = _shoppingCart.GetShoppingCartItems();
            if (!items.Any())
                return RedirectToAction("ShoppingCart");

            // 2. Tạo txnRef duy nhất (ví dụ dùng ticks)
            string txnRef = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            // 3. Lưu tạm giỏ và txnRef vào session
            HttpContext.Session.SetString("CartForPayment", JsonSerializer.Serialize(items));
            HttpContext.Session.SetString("VNPay_TxnRef", txnRef);

            // 4. Tạo fakeOrder chỉ để truyền TotalAmount
            var fakeOrder = new Order
            {
                Id = 0,
                TotalAmount = (decimal)_shoppingCart.GetShoppingCartTotal()
            };

            // 5. Build URL VNPay với txnRef duy nhất
            var vnpUrl = _vnPayService.CreatePaymentUrl(fakeOrder, HttpContext, txnRef);
            return Redirect(vnpUrl);
        }

        [HttpGet]
        public async Task<IActionResult> VNPayReturn()
        {
            // 1. Đọc và validate checksum
            var q = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
            string receivedHash = q["vnp_SecureHash"];
            q.Remove("vnp_SecureHash");
            q.Remove("vnp_SecureHashType");

            var signData = string.Join("&", q.OrderBy(x => x.Key)
                .Select(x => $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}"));
            if (VNPayHelper.HmacSHA512(_config["Vnpay:HashSecret"]!, signData) != receivedHash)
            {
                TempData["Error"] = "Checksum không hợp lệ";
                return RedirectToAction("ShoppingCart");
            }

            // 2. Chỉ xử lý khi thanh toán thành công
            if (q["vnp_ResponseCode"] == "00")
            {
                // 3. Lấy txnRef và giỏ hàng từ session
                string sessionTxn = HttpContext.Session.GetString("VNPay_TxnRef");
                string sessionCart = HttpContext.Session.GetString("CartForPayment");
                if (sessionTxn != q["vnp_TxnRef"] || string.IsNullOrEmpty(sessionCart))
                    return RedirectToAction("ShoppingCart");

                var items = JsonSerializer.Deserialize<List<ShoppingCartItem>>(sessionCart);
                if (items == null || !items.Any())
                    return RedirectToAction("ShoppingCart");

                // 4. Transaction: tạo đơn + xóa giỏ
                using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var email = User.FindFirstValue(ClaimTypes.Email)!;

                    await _ordersService.StoreOrderAsync(items, userId, email);
                    await _shoppingCart.ClearShoppingCartAsync();

                    await tx.CommitAsync();

                    // 5. Xóa session tạm
                    HttpContext.Session.Remove("CartForPayment");
                    HttpContext.Session.Remove("VNPay_TxnRef");

                    // 6. Hiển thị trang hoàn tất
                    var recommended = await _coursesService.GetRecommendedCoursesAsync(userId);
                    return View("OrderCompleted", new ShoppingCartVM
                    {
                        ShoppingCart = _shoppingCart,
                        ShoppingCartTotal = 0,
                        RecommendedCourses = recommended.ToList()
                    });
                }
                catch
                {
                    // rollback tự động, giỏ vẫn giữ
                    return RedirectToAction("ShoppingCart");
                }
            }

            TempData["Error"] = "Thanh toán không thành công";
            return RedirectToAction("ShoppingCart");
        }

        [HttpPost]
        [Route("Orders/VNPayIpn")]
        public async Task<IActionResult> VNPayIpn()
        {
            var q = Request.Form.ToDictionary(k => k.Key, v => v.Value.ToString());
            var received = q["vnp_SecureHash"];
            q.Remove("vnp_SecureHash");
            q.Remove("vnp_SecureHashType");

            // Xác thực checksum
            var calc = VNPayHelper.HmacSHA512(
                _config["Vnpay:HashSecret"]!,
                string.Join("&", q.OrderBy(x => x.Key).Select(x => $"{x.Key}={x.Value}"))
            );
            if (calc != received)
                return Content("97"); // sai chữ ký

            // Nếu thanh toán thành công
            if (q["vnp_ResponseCode"] == "00")
            {
                // lưu đơn và clear cart
                var items = _shoppingCart.GetShoppingCartItems();
                var userId = q["vnp_TxnRef"]; // hoặc lưu reference từ order đã tạo
                var email = User.FindFirstValue(ClaimTypes.Email);
                await _ordersService.StoreOrderAsync(items, userId, email!);
                await _shoppingCart.ClearShoppingCartAsync();

                return Content("00"); // OK
            }

            return Content("01"); // fail
        }
    }
}

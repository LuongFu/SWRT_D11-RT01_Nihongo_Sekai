using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;
        public OrdersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Course).Include(n => n.User).ToListAsync();

            if(userRole != "Admin")
            {
                orders = orders.Where(n => n.UserId == userId).ToList();
            }

            return orders;
        }

        public async Task<List<int>> GetPurchasedCourseIdsByUser(string userId)
        {
            return await _context.OrderItems
                .Where(o => o.Order.UserId == userId)
                .Select(o => o.CourseId)
                .Distinct()
                .ToListAsync();
        }


        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    CourseId = item.Course.Id,
                    OrderId = order.Id,
                    Price = item.Course.FinalPrice
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();
        }
    }
}

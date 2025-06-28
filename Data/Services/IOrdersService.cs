using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.Services
{
    public interface IOrdersService
    {
        Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);

        Task<List<int>> GetPurchasedCourseIdsByUser(string userId);

    }
}

using JapaneseLearningPlatform.Data.Cart;
using JapaneseLearningPlatform.Models;
namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ShoppingCartVM
    {
        public ShoppingCart ShoppingCart { get; set; }
        public double ShoppingCartTotal { get; set; }
        public List<CourseListItemVM> RecommendedCourses { get; set; }
    }
}

using JapaneseLearningPlatform.Models;

namespace NihongoSekaiPlatform.Data.ViewModels
{
    public class CourseWithPurchaseVM
    {
        public Course? Course { get; set; }
        public bool IsInCart { get; set; }
        public bool IsPurchased { get; set; }
        public double FinalPrice => Course.FinalPrice;
        public int? DiscountPercent => Course?.DiscountPercent;
    }
}

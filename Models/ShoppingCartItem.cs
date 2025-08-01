using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        public Course Course { get; set; }
        public int CourseId { get; set; }
        public int Amount { get; set; }


        public string ShoppingCartId { get; set; }
    }
}

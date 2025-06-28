using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class NewCourseVM : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Course name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Course description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "Price in $")]
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Display(Name = "Course poster URL")]
        [Required(ErrorMessage = "Course poster URL is required")]
        public string ImageURL { get; set; }
        [Display(Name = "Discount Percent")]
        [Required(ErrorMessage = "Discount Percent is required. If you want, you can set to 0%.")]
        public int? DiscountPercent { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DiscountPercent < 0 || DiscountPercent > 99)
            {
                yield return new ValidationResult("Discount must be between 0% and 99%.", new[] { "DiscountPercent" });
            }
        }

        [Display(Name = "Course start date")]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Course end date")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Select a category")]
        [Required(ErrorMessage = "Course category is required")]
        public CourseCategory CourseCategory { get; set; }

        [Display(Name = "Select video(s)")]
        [Required(ErrorMessage = "Course video(s) is required")]
        public List<int> VideoIds { get; set; }
    }
}

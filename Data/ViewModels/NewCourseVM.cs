using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class NewCourseVM : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Tên khóa học")]
        [Required(ErrorMessage = "Bắt buộc điền")]
        public string Name { get; set; }

        [Display(Name = "Mô tả khóa học")]
        [Required(ErrorMessage = "Bắt buộc điền")]
        public string Description { get; set; }

        [Display(Name = "Giá tính theo VND")]
        [Required(ErrorMessage = "Bắt buộc điền")]
        public double Price { get; set; }

        [Display(Name = "URL Ảnh bìa khóa học")]
        [Required(ErrorMessage = "Bắt buộc điền")]
        public string ImageURL { get; set; }
        [Display(Name = "Phần trăm giảm giá")]
        [Required(ErrorMessage = "Bắt buộc điền. Nếu không giảm giá, hãy điền 0.")]
        public int? DiscountPercent { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DiscountPercent < 0 || DiscountPercent > 99)
            {
                yield return new ValidationResult("Phần trăm giảm giá phải nằm trong khoảng từ 0% đến 99%.", new[] { "DiscountPercent" });
            }
        }

        [Display(Name = "Thời gian áp dụng")]
        [Required(ErrorMessage = "Bắt buộc điền")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Hạn hết giảm giá")]
        [Required(ErrorMessage = "Bắt buộc điền")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Chọn phân loại")]
        [Required(ErrorMessage = "Bắt buộc chọn")]
        public CourseCategory CourseCategory { get; set; }

        [Display(Name = "Chọn video")]
        [Required(ErrorMessage = "Bắt buộc chọn")]
        public List<int> VideoIds { get; set; }
    }
}

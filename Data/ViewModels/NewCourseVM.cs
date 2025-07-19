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
        public int Price { get; set; }

        [Display(Name = "URL Ảnh bìa khóa học")]
        [Required(ErrorMessage = "Bắt buộc điền")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageURL { get; set; } // Đường dẫn được lưu sau khi upload
        [Display(Name = "Phần trăm giảm giá")]
        [Required(ErrorMessage = "Bắt buộc điền. Nếu không giảm giá, hãy điền 0.")]
        public int? DiscountPercent { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DiscountPercent < 0 || DiscountPercent > 99)
            {
                yield return new ValidationResult("Phần trăm giảm giá phải nằm trong khoảng từ 0% đến 99%.", new[] { "DiscountPercent" });
            }
            if (Price < 1000 || Price > 4999000)
            {
                yield return new ValidationResult(
                    "Giá phải từ 1,000 VND đến 4,999,000 VND.",
                    new[] { "Price" });
            }

            // Kiểm tra Price có kết thúc bằng số 0 (hàng đơn vị bằng 0)
            if (Price % 10 != 0)
            {
                yield return new ValidationResult(
                    "Giá phải có hàng đơn vị bằng 0 (ví dụ: 1,230 hoặc 4,500).",
                    new[] { "Price" });
            }
            if (StartDate >= EndDate)
            {
                yield return new ValidationResult(
                    "Thời gian bắt đầu giảm giá phải nhỏ hơn thời gian kết thúc giảm giá.",
                    new[] { "StartDate", "EndDate" });
            }
            if (StartDate.Date < DateTime.Now.Date)
            {
                yield return new ValidationResult(
                    "Thời gian bắt đầu không được nhỏ hơn ngày hiện tại.",
                    new[] { "StartDate" });
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
    }
}

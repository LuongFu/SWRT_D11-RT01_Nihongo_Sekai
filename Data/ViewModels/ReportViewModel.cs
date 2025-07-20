using JapaneseLearningPlatform.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ReportViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn chủ đề.")]
        public ReportSubject Subject { get; set; }

        public string? OrderNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung tin nhắn.")]
        [MaxLength(2000, ErrorMessage = "Tối đa 2000 ký tự.")]
        public string Message { get; set; } = null!;

        // Bind recaptcha token from form
        [BindProperty]
        [Required(ErrorMessage = "Vui lòng xác minh rằng bạn không phải robot.")]
        public string RecaptchaToken { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // 1) If billing, order number is required
            if (Subject == ReportSubject.Billing)
            {
                if (string.IsNullOrWhiteSpace(OrderNumber))
                {
                    yield return new ValidationResult(
                        "Vui lòng nhập mã đơn hàng.",
                        new[] { nameof(OrderNumber) }
                    );
                }
            }            
        }
    }
}

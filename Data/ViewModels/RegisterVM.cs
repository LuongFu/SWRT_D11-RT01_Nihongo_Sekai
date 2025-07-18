using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class RegisterVM : IValidatableObject
    {
        [Required(ErrorMessage = "Tên đầy đủ là bắt buộc.")]
        [Display(Name = "Tên đầy đủ")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không trùng khớp.")]
        public string ConfirmPassword { get; set; } = null!;

        [Display(Name = "Đăng ký làm đối tác")]
        public bool ApplyAsPartner { get; set; }

        [Display(Name = "Năm kinh nghiệm")]
        public YearsOfExperience? YearsOfExperience { get; set; }

        [Display(Name = "Chuyên môn")]
        public List<SpecializationType> Specializations { get; set; } = new();

        [Display(Name = "Chứng chỉ")]
        public List<IFormFile> PartnerDocument { get; set; } = new();

        [Required(ErrorMessage = "Bạn phải đồng ý các điều khoản!")]
        [Display(Name = "Đồng ý các điều khoản")]
        public bool AgreeTerms { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Must accept terms
            if (!AgreeTerms)
            {
                yield return new ValidationResult(
                    "Bạn phải đồng ý với các điều khoản và điều kiện.",
                    new[] { nameof(AgreeTerms) });
            }

            // If applying as partner, require extra fields
            if (ApplyAsPartner)
            {
                if (!YearsOfExperience.HasValue)
                {
                    yield return new ValidationResult(
                        "Vui lòng chọn năm kinh nghiệm.",
                        new[] { nameof(YearsOfExperience) });
                }

                if (Specializations == null || !Specializations.Any())
                {
                    yield return new ValidationResult(
                        "Chọn ít nhất một chuyên môn.",
                        new[] { nameof(Specializations) });
                }

                if (PartnerDocument == null || !PartnerDocument.Any())
                {
                    yield return new ValidationResult(
                        "Tải lên ít nhất một file tài liệu.",
                        new[] { nameof(PartnerDocument) });
                }
            }
        }
    }
}

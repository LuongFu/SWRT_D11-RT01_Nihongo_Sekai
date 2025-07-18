using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Bắt buộc điền email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Bắt buộc điền mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}

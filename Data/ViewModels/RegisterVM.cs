using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NihongoSekaiWebApplication_D11_RT01.Data.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }
        public bool ApplyAsPartner { get; set; }

        [Display(Name = "Verification File")]
        public IFormFile? PartnerDocument { get; set; }  // Required only if ApplyAsPartner = true
    }
}

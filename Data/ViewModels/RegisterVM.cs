using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class RegisterVM : IValidatableObject
    {
        [Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Display(Name = "Apply as Partner")]
        public bool ApplyAsPartner { get; set; }

        [Display(Name = "Years of Experience")]
        public YearsOfExperience? YearsOfExperience { get; set; }

        [Display(Name = "Specializations")]
        public List<SpecializationType> Specializations { get; set; } = new();

        [Display(Name = "Certificates & Qualifications")]
        public List<IFormFile> PartnerDocument { get; set; } = new();

        [Required(ErrorMessage = "You need to respect our terms!")]
        [Display(Name = "Agree to Terms")]
        public bool AgreeTerms { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Must accept terms
            if (!AgreeTerms)
            {
                yield return new ValidationResult(
                    "You must agree to the Terms & Conditions.",
                    new[] { nameof(AgreeTerms) });
            }

            // If applying as partner, require extra fields
            if (ApplyAsPartner)
            {
                if (!YearsOfExperience.HasValue)
                {
                    yield return new ValidationResult(
                        "Please select your years of experience.",
                        new[] { nameof(YearsOfExperience) });
                }

                if (Specializations == null || !Specializations.Any())
                {
                    yield return new ValidationResult(
                        "Select at least one specialization.",
                        new[] { nameof(Specializations) });
                }

                if (PartnerDocument == null || !PartnerDocument.Any())
                {
                    yield return new ValidationResult(
                        "Upload at least one verification document.",
                        new[] { nameof(PartnerDocument) });
                }
            }
        }
    }
}

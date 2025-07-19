using System;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class EditProfileVM
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 digits.")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters.")]
        public string Gender { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "Job Name cannot exceed 100 characters.")]
        public string JobName { get; set; }

        [Url(ErrorMessage = "Facebook link must be a valid URL.")]
        [StringLength(200, ErrorMessage = "Facebook link cannot exceed 200 characters.")]
        public string Facebook { get; set; }

        [Url(ErrorMessage = "YouTube link must be a valid URL.")]
        [StringLength(200, ErrorMessage = "YouTube link cannot exceed 200 characters.")]
        public string YouTube { get; set; }

        public string ProfilePicturePath { get; set; }

        public bool ShowDeleteButton { get; set; }
    }
}

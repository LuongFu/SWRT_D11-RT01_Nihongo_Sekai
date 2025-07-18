using JapaneseLearningPlatform.Models.Partner;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name = "Tên đầy đủ")]
        public string? FullName { get; set; }
        public string Role { get; set; } = "Learner";
        public bool IsApproved { get; set; } = false;
        //public string? PartnerDocumentPath { get; set; }
        public bool IsBanned { get; set; } = false;
        public string? ProfilePicturePath { get; set; }
        public PartnerProfile? PartnerProfile { get; set; }
    }
}

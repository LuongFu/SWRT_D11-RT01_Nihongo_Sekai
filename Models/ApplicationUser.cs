using JapaneseLearningPlatform.Models.Partner;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name = "Full name")]
        public string? FullName { get; set; }
        public string Role { get; set; } = "Learner";
        public bool IsApproved { get; set; } = false;
        //public string? PartnerDocumentPath { get; set; }
        public bool IsBanned { get; set; } = false;
        public string? ProfilePicturePath { get; set; }
        public PartnerProfile? PartnerProfile { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Description { get; set; }
        public string? JobName { get; set; }
        public string? Facebook { get; set; }
        public string? YouTube { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name = "Full name")]
        public string? FullName { get; set; }
        public string Role { get; set; } = "Learner";
        public bool IsApproved { get; set; } = false;
        public string? PartnerDocumentPath { get; set; }
    }
}

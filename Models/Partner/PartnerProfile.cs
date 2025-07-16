using JapaneseLearningPlatform.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearningPlatform.Models.Partner
{
    public class PartnerProfile
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public PartnerStatus Status { get; set; } = PartnerStatus.Pending;
        public DateTime? DecisionAt { get; set; }

        [Required]
        public YearsOfExperience YearsOfExperience { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }    // Ngày khởi tạo hồ sơ (ngày nhận đơn)

        public ICollection<PartnerSpecialization> Specializations { get; set; }
            = new List<PartnerSpecialization>();

        public ICollection<PartnerDocument> Documents { get; set; }
            = new List<PartnerDocument>();
    }
}

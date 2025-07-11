using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearningPlatform.Models.Partner
{
    public class PartnerProfile
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }          // ← thêm “= null!;”
            = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }   // ← thêm “= null!;”
            = null!;

        public PartnerStatus Status { get; set; } = PartnerStatus.Pending;
        public DateTime? DecisionAt { get; set; }

        [Required]
        public YearsOfExperience YearsOfExperience { get; set; }

        // ← khởi tạo collection ngay tại đây
        public ICollection<PartnerSpecialization> Specializations { get; set; }
            = new List<PartnerSpecialization>();

        public ICollection<PartnerDocument> Documents { get; set; }
            = new List<PartnerDocument>();
    }
}

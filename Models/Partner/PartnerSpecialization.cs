using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearningPlatform.Models.Partner
{
    public class PartnerSpecialization
    {
        public int Id { get; set; }

        [Required]
        public int PartnerProfileId { get; set; }

        [ForeignKey(nameof(PartnerProfileId))]
        public PartnerProfile PartnerProfile { get; set; }  // ← thêm “= null!;”
            = null!;

        [Required]
        public SpecializationType Specialization { get; set; }
    }
}

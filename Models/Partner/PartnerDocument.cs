using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearningPlatform.Models.Partner
{
    public class PartnerDocument
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }          // ← thêm “= null!;”
            = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }   // ← thêm “= null!;”
            = null!;

        [Required, MaxLength(500)]
        public string FilePath { get; set; }        // ← thêm “= null!;”
            = null!;

        [Required]
        public int PartnerProfileId { get; set; }

        [ForeignKey(nameof(PartnerProfileId))]
        public PartnerProfile Profile { get; set; } = null!;

    }
}

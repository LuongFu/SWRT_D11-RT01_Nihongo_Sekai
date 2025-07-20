using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearningPlatform.Models
{
    public class ClassroomChatMessage
    {
        public int Id { get; set; }

        [Required]
        public int ClassroomInstanceId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ApplicationUser User { get; set; }
        public ClassroomInstance ClassroomInstance { get; set; }
    }
}

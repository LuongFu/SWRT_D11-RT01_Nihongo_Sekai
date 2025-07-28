using System;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class PrivateChatMessage
    {
        public int Id { get; set; }

        [Required]
        public int ClassroomInstanceId { get; set; }

        [Required]
        public string UserId { get; set; }  // Người gửi

        [Required]
        public string TargetUserId { get; set; }  // Người nhận

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow; // Gắn UTC mặc định

        // Navigation
        public ApplicationUser User { get; set; }
        public ApplicationUser TargetUser { get; set; }
        public ClassroomInstance ClassroomInstance { get; set; }
    }
}

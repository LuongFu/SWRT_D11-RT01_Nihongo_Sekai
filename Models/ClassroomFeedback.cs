using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class ClassroomFeedback
    {
        public int Id { get; set; }

        // Liên kết với ClassroomInstance
        [Required]
        public int ClassroomInstanceId { get; set; }
        public ClassroomInstance ClassroomInstance { get; set; }

        // Liên kết với Learner (ApplicationUser)
        [Required]
        public string LearnerId { get; set; }
        public ApplicationUser Learner { get; set; }

        // Đánh giá số (1-5)
        [Range(1, 5)]
        public int Rating { get; set; }

        // Nội dung nhận xét
        [MaxLength(1000)]
        public string Comment { get; set; }

        // Thời gian tạo và cập nhật
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}

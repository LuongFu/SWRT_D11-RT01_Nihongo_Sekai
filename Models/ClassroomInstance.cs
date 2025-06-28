using JapaneseLearningPlatform.Data.Enums;

namespace JapaneseLearningPlatform.Models
{
    public class ClassroomInstance
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public ClassroomTemplate Template { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan ClassTime { get; set; }             // Giờ học mỗi buổi
        public int MaxCapacity { get; set; }                // Số lượng học viên tối đa
        public decimal Price { get; set; }                  // Giá lớp học
        public bool IsPaid { get; set; }                    // Miễn phí hay tính phí

        public string? GoogleMeetLink { get; set; }         // Link lớp học
        public ClassroomStatus Status { get; set; }         // Upcoming, Ongoing, Completed, Cancelled
        public ICollection<ClassroomEnrollment> Enrollments { get; set; } = new List<ClassroomEnrollment>();
        public List<FinalAssessment>? Assessments { get; set; }
    }
}

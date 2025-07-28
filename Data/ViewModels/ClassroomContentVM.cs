using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomContentVM
    {
        public ClassroomInstance Instance { get; set; }
        public ClassroomTemplate Template { get; set; }
        public string? PartnerName { get; set; }
        public FinalAssignment? FinalAssignment { get; set; }
        public AssignmentSubmission? Submission { get; set; }
        public List<AssignmentSubmission>? AllSubmissions { get; set; }
        public bool HasSubmitted { get; set; }
        public bool HasReviewed { get; set; }
        public List<ClassroomResource>? Resources { get; set; }
        public List<ClassroomFeedback>? Feedbacks { get; set; } = new List<ClassroomFeedback>();
        public ClassroomFeedback? UserFeedback { get; set; }

        // === Chat Properties ===
        public string CurrentUserId { get; set; }  // ID của người dùng hiện tại
        public string CurrentUserName { get; set; } // Tên người dùng hiện tại
        public List<ApplicationUser>? ChatMembers { get; set; } // Danh sách thành viên trong lớp để hiển thị chat riêng
    }

}

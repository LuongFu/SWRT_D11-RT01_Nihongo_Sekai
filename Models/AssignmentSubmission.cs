namespace JapaneseLearningPlatform.Models
{
    public class AssignmentSubmission
    {
        public int Id { get; set; }

        public int FinalAssignmentId { get; set; }
        public FinalAssignment Assignment { get; set; }

        public string LearnerId { get; set; }
        public ApplicationUser Learner { get; set; }

        public string? FilePath { get; set; }               // Đường dẫn bài nộp (file upload)
        public string? AnswerText { get; set; }             // Nội dung bài làm tự luận (tùy chọn)

        public DateTime SubmittedAt { get; set; }
        public double? Score { get; set; }
        public string? Feedback { get; set; }
    }

}

namespace JapaneseLearningPlatform.Models
{
    public class AssessmentSubmission
    {
        public int Id { get; set; }

        public int FinalAssessmentId { get; set; }
        public FinalAssessment Assessment { get; set; }

        public string LearnerId { get; set; }
        public ApplicationUser Learner { get; set; }

        public string? FilePath { get; set; }               // Đường dẫn bài nộp (file upload)
        public string? AnswerText { get; set; }             // Nội dung bài làm tự luận (tùy chọn)

        public DateTime SubmittedAt { get; set; }
        public double? Score { get; set; }
        public string? Feedback { get; set; }
    }

}

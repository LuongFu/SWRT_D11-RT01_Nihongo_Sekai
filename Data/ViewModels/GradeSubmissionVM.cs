namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class GradeSubmissionVM
    {
        public int SubmissionId { get; set; }
        public int InstanceId { get; set; }
        public string? LearnerName { get; set; }
        public string? AnswerText { get; set; }
        public string? FilePath { get; set; }
        public double? Score { get; set; }
        public string? Feedback { get; set; }
        public string? ClassTitle { get; set; }
        public string? PartnerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan ClassTime { get; set; }
        public string? GoogleMeetLink { get; set; }

    }
}

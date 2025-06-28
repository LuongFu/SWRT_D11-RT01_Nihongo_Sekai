namespace JapaneseLearningPlatform.Models
{
    public class ClassroomEvaluation
    {
        public int Id { get; set; }
        public int InstanceId { get; set; }
        public ClassroomInstance Instance { get; set; }
        public string LearnerId { get; set; }
        public ApplicationUser Learner { get; set; }
        public double? Score { get; set; }                  // Điểm tổng kết
        public string? Comment { get; set; }                // Nhận xét cá nhân
        public DateTime EvaluatedAt { get; set; }
    }

}

namespace JapaneseLearningPlatform.Models
{
    public class FinalAssessment
    {
        public int Id { get; set; }

        public int ClassroomInstanceId { get; set; }
        public ClassroomInstance Instance { get; set; }

        public string Instructions { get; set; }            // Hướng dẫn làm bài kiểm tra
        public DateTime? DueDate { get; set; }

        public ICollection<AssessmentSubmission>? Submissions { get; set; }
    }
}

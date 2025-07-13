namespace JapaneseLearningPlatform.Models
{
    public class FinalAssignment
    {
        public int Id { get; set; }

        public int ClassroomInstanceId { get; set; }
        public ClassroomInstance Instance { get; set; }

        public string Instructions { get; set; }            // Hướng dẫn làm bài kiểm tra
        public DateTime? DueDate { get; set; }

        public List<AssignmentSubmission>? Submissions { get; set; }
    }
}

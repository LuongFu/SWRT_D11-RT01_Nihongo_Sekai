namespace JapaneseLearningPlatform.Models
{
    public class CourseContentProgress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public int ContentItemId { get; set; } // Video hoặc Quiz
        public bool IsCompleted { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    }

}

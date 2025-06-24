namespace JapaneseLearningPlatform.Models
{
    public class Video_Course
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int VideoId { get; set; }
        public Video Video { get; set; }
    }
}

using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class CourseDetailVM
    {
        public Course? Course { get; set; }
        public bool IsPurchased { get; set; }
        public bool IsInCart { get; set; }
        public List<Video>? Videos { get; set; }
    }
}

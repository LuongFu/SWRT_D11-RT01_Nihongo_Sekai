using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class RatingCreateVM
    {
        public int CourseId { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
    }

}

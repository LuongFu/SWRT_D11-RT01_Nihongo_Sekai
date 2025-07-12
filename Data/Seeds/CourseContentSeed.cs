using Microsoft.AspNetCore.Mvc;
using JapaneseLearningPlatform.Data.Enums;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class CourseContentSeed
    {
        public string Title { get; set; }
        public int SectionIndex { get; set; }
        public ContentType Type { get; set; }
        public int? VideoId { get; set; }
        public int? QuizIndex { get; set; } // Chỉ là index để map sau
    }

}

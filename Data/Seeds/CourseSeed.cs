using JapaneseLearningPlatform.Data.Enums;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class CourseSeed
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public double Price { get; set; }
        public string ImageURL { get; set; } = "";
        public int? Discount { get; set; }
        public CourseCategory Category { get; set; }
        public string[] Sections { get; set; } = Array.Empty<string>();
        public string QuizTitle { get; set; } = "";
        public QuizQuestion[] Questions { get; set; } = Array.Empty<QuizQuestion>();
        public List<CourseContentSeed> Contents { get; set; } = new();
    }
}

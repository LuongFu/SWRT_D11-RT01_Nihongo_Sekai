using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class CourseHierarchyVM
    {
        public Course Course { get; set; }
        public List<CourseSection> Sections { get; set; }
        public bool IsPurchased { get; set; }
        public bool IsInCart { get; set; }
        public Dictionary<int, int> QuizHighScores { get; set; } = new();
        public Video Video { get; set; }
        public List<int> CompletedContentIds { get; set; } = new();
        public double ProgressPercent { get; set; }
    }
}

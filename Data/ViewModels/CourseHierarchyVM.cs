using JapaneseLearningPlatform.Models;

namespace NihongoSekaiPlatform.Data.ViewModels
{
    public class CourseHierarchyVM
    {
        public Course Course { get; set; }
        public List<CourseSection> Sections { get; set; }
        public bool IsPurchased { get; set; }
        public bool IsInCart { get; set; }
    }
}

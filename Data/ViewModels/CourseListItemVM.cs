namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class CourseListItemVM
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public double Tuition { get; set; }
        public string Level { get; set; }
    }
}

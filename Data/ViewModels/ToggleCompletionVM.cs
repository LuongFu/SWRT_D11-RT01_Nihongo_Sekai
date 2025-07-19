namespace JapaneseLearningPlatform.Data.ViewModels
{

    public class ToggleCompletionVM
    {
        public int CourseId { get; set; }
        public int ContentItemId { get; set; }
        public bool IsCompleted { get; set; }
    }
}

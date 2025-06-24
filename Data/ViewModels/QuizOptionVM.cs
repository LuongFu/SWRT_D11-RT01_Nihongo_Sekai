namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class QuizOptionVM
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; } // Chỉ dùng để kiểm tra sau
    }
}

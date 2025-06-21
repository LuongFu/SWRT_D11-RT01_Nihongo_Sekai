namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class QuizResultDetail
    {
        public int Id { get; set; }

        public int QuizResultId { get; set; }
        public QuizResult QuizResult { get; set; }

        public int QuestionId { get; set; }
        public QuizQuestion Question { get; set; }

        public int? SelectedOptionId { get; set; }
        public QuizOption SelectedOption { get; set; }

        public bool IsCorrect { get; set; }
    }

}

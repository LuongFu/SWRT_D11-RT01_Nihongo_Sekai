namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class TakeQuizVM
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }

        public List<QuizQuestionVM> Questions { get; set; }
    }
}

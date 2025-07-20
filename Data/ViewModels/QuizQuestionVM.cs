using JapaneseLearningPlatform.Data.Enums;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class QuizQuestionVM
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }

        public List<QuizOptionVM> Options { get; set; }
        public QuestionType QuestionType { get; set; } // Add this

        // For SingleChoice
        public int? SelectedOptionId { get; set; }

        // For MultipleChoice
        public List<int> SelectedOptionIds { get; set; } = new();
        public string AIExplanation { get; set; }
    }
}

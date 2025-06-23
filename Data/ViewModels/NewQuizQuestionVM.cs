using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class NewQuizQuestionVM
    {
        public int? QuestionId { get; set; } // dùng cho edit
        public int QuizId { get; set; }

        [Required]
        public string? QuestionText { get; set; }

        public bool IsMultipleChoice { get; set; }

        public List<QuizOptionVM> Options { get; set; } = new();
    }

}

using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class QuizQuestionFormVM
    {
        public int? QuestionId { get; set; }
        public int QuizId { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        public List<QuizOptionVM> Options { get; set; } = new List<QuizOptionVM>();
    }


}

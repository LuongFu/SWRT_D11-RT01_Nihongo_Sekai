using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.Enums
{
    public enum QuestionType
    {
        [Display(Name = "Một đáp án")]
        SingleChoice = 0,

        [Display(Name = "Nhiều đáp án")]
        MultipleChoice = 1
    }
}

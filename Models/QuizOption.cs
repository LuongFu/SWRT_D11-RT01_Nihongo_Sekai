using JapaneseLearningPlatform.Data.Base;
using System.ComponentModel.DataAnnotations;

public class QuizOption : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string OptionText { get; set; }

    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }
    public QuizQuestion Question { get; set; }
}

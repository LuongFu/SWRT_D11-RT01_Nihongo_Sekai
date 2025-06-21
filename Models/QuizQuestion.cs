using JapaneseLearningPlatform.Data.Base;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations;

public class QuizQuestion : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string QuestionText { get; set; }

    public int QuizId { get; set; }
    public Quiz Quiz { get; set; }

    public List<QuizOption> Options { get; set; }
}

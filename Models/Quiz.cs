using JapaneseLearningPlatform.Data.Base;
using System.ComponentModel.DataAnnotations;

public class Quiz : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    // Navigation
    public List<QuizQuestion> Questions { get; set; }
}

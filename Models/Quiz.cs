using JapaneseLearningPlatform.Data.Base;
using JapaneseLearningPlatform.Models;
using System.ComponentModel.DataAnnotations;

public class Quiz : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }
    // Foreign key
    public int CourseId { get; set; }
    public Course Course { get; set; }

    // Navigation
    public List<QuizQuestion> Questions { get; set; }
}

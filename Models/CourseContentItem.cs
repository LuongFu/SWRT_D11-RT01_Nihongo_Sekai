using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Base;
using JapaneseLearningPlatform.Models;
using System.ComponentModel.DataAnnotations;

public class CourseContentItem : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public ContentType ContentType { get; set; }

    // Foreign Keys
    public int SectionId { get; set; }
    public CourseSection Section { get; set; }

    // Optional links
    public int? VideoId { get; set; }
    public Video Video { get; set; }

    public int? QuizId { get; set; }
    public Quiz Quiz { get; set; }
    [Required]
    public int DisplayOrder { get; internal set; }
}

using JapaneseLearningPlatform.Data.Base;
using JapaneseLearningPlatform.Models;
using System.ComponentModel.DataAnnotations;

public class CourseSection : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    // FK
    public int CourseId { get; set; }
    public Course Course { get; set; }

    // Navigation
    public List<CourseContentItem> ContentItems { get; set; }
}

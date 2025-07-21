using JapaneseLearningPlatform.Models;

public class CourseRating
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string UserId { get; set; } = default!;
    public int Stars { get; set; }    // 1–5
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ApplicationUser User { get; set; } = default!;
    public Course Course { get; set; } = default!;
}


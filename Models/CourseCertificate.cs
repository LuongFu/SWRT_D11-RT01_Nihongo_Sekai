using JapaneseLearningPlatform.Models;

public class CourseCertificate
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string UserId { get; set; } = default!;
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public string FileUrl { get; set; } = default!;   // đường dẫn PDF/PNG
    public ApplicationUser User { get; set; } = default!;
    public Course Course { get; set; } = default!;
}

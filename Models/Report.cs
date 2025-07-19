using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;

public class Report
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = null!;

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public ReportSubject Subject { get; set; }   // Enum → int

    public string? OrderNumber { get; set; }

    [Required, MaxLength(2000)]
    public string Message { get; set; } = null!;

    [Required]
    public string Role { get; set; } = null!;

    public DateTime SubmittedAt { get; set; }

    public bool IsResolved { get; set; } = false;
}

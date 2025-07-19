using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ReportViewModel
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public ReportSubject Subject { get; set; }

        public string? OrderNumber { get; set; }

        [Required, MaxLength(2000)]
        public string Message { get; set; } = null!;
    }
}

using JapaneseLearningPlatform.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public ReportSubject Subject { get; set; }

        public string? OrderNumber { get; set; }

        [Required, MaxLength(2000)]
        public string Message { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;         // Learner / Partner / Guest

        public DateTime SubmittedAt { get; set; }         // thời điểm gửi

        public bool IsResolved { get; set; } = false;     // mặc định chưa xử lý
    }
}

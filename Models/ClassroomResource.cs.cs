using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearningPlatform.Models
{
    public class ClassroomResource
    {
        public int Id { get; set; }

        [Required]
        public int ClassroomInstanceId { get; set; }

        [ForeignKey(nameof(ClassroomInstanceId))]
        public ClassroomInstance ClassroomInstance { get; set; } = null!;

        [Required, MaxLength(255)]
        public string FileName { get; set; } = null!;

        [Required]
        public string FilePath { get; set; } = null!;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}

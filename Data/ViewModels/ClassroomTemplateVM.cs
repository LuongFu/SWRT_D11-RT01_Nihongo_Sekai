using JapaneseLearningPlatform.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomTemplateVM
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? ImageURL { get; set; }

        public IFormFile? ImageFile { get; set; }  // 🖼 Upload ảnh

        [Required]
        public LanguageLevel LanguageLevel { get; set; }

        [Required]
        public double SessionTime { get; set; } // Giờ

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, 10000000)]
        public decimal Price { get; set; }

        public string Status { get; set; } = "Draft";
    }
}

using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Models;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomTemplateVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tiêu đề mẫu")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Mô tả mẫu")]
        public string Description { get; set; }

        [Display(Name = "Ảnh đại diện (tùy chọn)")]
        public string? ImageURL { get; set; }

        [Display(Name = "Tải ảnh (tùy chọn)")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Tài liệu đính kèm (tùy chọn)")]
        public IFormFile? DocumentFile { get; set; }

        [Display(Name = "Tài liệu đã tải")]
        public string? DocumentURL { get; set; } // ✅ BỔ SUNG để không bị lỗi

        [Required]
        [Display(Name = "Trình độ ngôn ngữ")]
        public LanguageLevel LanguageLevel { get; set; }

        // Chuyển từ VM xuống Entity
        public ClassroomTemplate ToEntity(string partnerId, string? imagePath, string? documentPath)
        {
            return new ClassroomTemplate
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                ImageURL = imagePath,
                DocumentURL = documentPath,
                LanguageLevel = this.LanguageLevel,
                PartnerId = partnerId
            };
        }

        // Chuyển từ Entity lên VM
        public static ClassroomTemplateVM FromEntity(ClassroomTemplate entity)
        {
            return new ClassroomTemplateVM
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                ImageURL = entity.ImageURL,
                DocumentURL = entity.DocumentURL, // ✅ Gán đúng chiều ngược lại
                LanguageLevel = entity.LanguageLevel
            };
        }
    }
}

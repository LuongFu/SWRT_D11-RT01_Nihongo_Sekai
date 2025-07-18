﻿using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Models;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomTemplateVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Template Name")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Thumbnail (optional)")]
        public string? ImageURL { get; set; }

        [Display(Name = "Upload thumbnail (optional)")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Document File (optional)")]
        public IFormFile? DocumentFile { get; set; }

        [Display(Name = "Tài liệu đã tải")]
        public string? DocumentURL { get; set; } // ✅ BỔ SUNG để không bị lỗi

        [Required]
        [Display(Name = "Level")]
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

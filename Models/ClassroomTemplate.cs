﻿using JapaneseLearningPlatform.Data.Enums;

namespace JapaneseLearningPlatform.Models
{
    public class ClassroomTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }                  // Tên lớp học (vd: Hội thoại cơ bản)
        public string Description { get; set; }            // Mô tả chi tiết lớp học
        public string? ImageURL { get; set; }              // Ảnh đại diện lớp học
        public LanguageLevel LanguageLevel { get; set; }
        public string PartnerId { get; set; }              // Người tạo lớp học (Partner)
        public ApplicationUser Partner { get; set; }
        public List<ClassroomInstance>? Instances { get; set; } // Các lần mở lớp từ template này
    }
}

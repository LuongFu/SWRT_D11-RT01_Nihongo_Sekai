using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JapaneseLearningPlatform.Data.Enums;

namespace JapaneseLearningPlatform.Models
{
    public class ClassroomTemplate
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }              // Tên lớp học

        [Required]
        public string Description { get; set; }        // Mô tả chi tiết lớp học

        public string? ImageURL { get; set; }          // Ảnh đại diện lớp học

        public string? DocumentURL { get; set; }       // Tài liệu đính kèm (PDF, slides, v.v.)

        [Required]
        public LanguageLevel LanguageLevel { get; set; }  // Trình độ ngôn ngữ
        public double SessionTime { get; set; } // Số giờ học mỗi buổi

        // Quan hệ đến Partner
        public string PartnerId { get; set; }
        public ApplicationUser Partner { get; set; }

        // Danh sách các phiên học sử dụng Template này
        public List<ClassroomInstance>? Instances { get; set; }
    }
}

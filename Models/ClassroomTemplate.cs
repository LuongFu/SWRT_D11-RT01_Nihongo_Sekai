using System.ComponentModel.DataAnnotations;
using JapaneseLearningPlatform.Data.Enums;

namespace JapaneseLearningPlatform.Models
{
    public class ClassroomTemplate
    {
        public int Id { get; set; }

        public string Title { get; set; }                  // Tên lớp học
        public string Description { get; set; }            // Mô tả chi tiết lớp học
        public string? ImageURL { get; set; }              // Ảnh đại diện lớp học

        public LanguageLevel LanguageLevel { get; set; }

        [Required]
        public double SessionTime { get; set; } // Ví dụ: 30 = 30 giờ

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }            // 🆕 Ngày bắt đầu lớp

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }              // 🆕 Ngày kết thúc lớp

        [Required]
        [Range(0, 10000000)]
        public decimal Price { get; set; }                 // 🆕 Giá tiền lớp học

        public string Status { get; set; } = "Draft";      // 🆕 Trạng thái (Draft / Published)

        public string PartnerId { get; set; }              // Người tạo lớp học
        public ApplicationUser Partner { get; set; }

        public List<ClassroomInstance>? Instances { get; set; }
    }
}

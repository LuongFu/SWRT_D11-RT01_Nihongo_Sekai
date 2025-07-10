using JapaneseLearningPlatform.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomInstanceVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mẫu lớp học là bắt buộc")]
        public int TemplateId { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Thời lượng mỗi buổi học là bắt buộc")]
        [Range(1, 24, ErrorMessage = "Thời lượng mỗi buổi học phải từ 1 đến 24 giờ")]
        public int SessionDurationHours { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        public decimal Price { get; set; }

        public string? GoogleMeetLink { get; set; }

        [Required(ErrorMessage = "Trạng thái lớp học là bắt buộc")]
        public ClassroomStatus Status { get; set; }

        // ✅ Thông tin từ Template để hiển thị
        public string? TemplateTitle { get; set; }
        public string? TemplateDescription { get; set; }
        public string? TemplateImageURL { get; set; }
        public string? LanguageLevel { get; set; }
        public string? DocumentURL { get; set; } // Tài liệu từ Template

        // ✅ Dữ liệu phụ trợ thống kê
        public int EnrollmentCount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsEnrolled { get; set; }  // Cho biết learner đã tham gia lớp chưa

    }
}

using JapaneseLearningPlatform.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomInstanceVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mẫu lớp học là bắt buộc")]
        [Display(Name = "Mẫu lớp học")]
        public int TemplateId { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Thời lượng mỗi buổi học là bắt buộc")]
        [Range(1, 24, ErrorMessage = "Thời lượng mỗi buổi học phải từ 1 đến 24 giờ")]
        [Display(Name = "Thời lượng buổi học (giờ)")]
        public int SessionDurationHours { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Display(Name = "Giá (VNĐ)")]
        [Range(1, 99999999, ErrorMessage = "Giá quá cao, vui lòng nhập giá thấp hơn 99tr")]
        public decimal Price { get; set; }

        [Display(Name = "Link Google Meet")]
        public string? GoogleMeetLink { get; set; }

        [Required(ErrorMessage = "Trạng thái lớp học là bắt buộc")]
        [Display(Name = "Trạng thái")]
        public ClassroomStatus Status { get; set; }

        // ✅ Thông tin từ Template để hiển thị
        [Display(Name = "Tiêu đề mẫu lớp")]
        public string? TemplateTitle { get; set; }

        [Display(Name = "Mô tả mẫu lớp")]
        public string? TemplateDescription { get; set; }

        [Display(Name = "Ảnh mẫu lớp")]
        public string? TemplateImageURL { get; set; }

        [Display(Name = "Trình độ ngôn ngữ")]
        public LanguageLevel LanguageLevel { get; set; }

        [Display(Name = "Tài liệu")]
        public string? DocumentURL { get; set; } // Tài liệu từ Template

        // ✅ Dữ liệu phụ trợ thống kê
        [Display(Name = "Số lượng học viên")]
        public int EnrollmentCount { get; set; }

        [Display(Name = "Lớp học trả phí")]
        public bool IsPaid { get; set; }

        [Display(Name = "Đã tham gia lớp học")]
        public bool IsEnrolled { get; set; }  // Cho biết learner đã tham gia lớp chưa
    }
}

using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class FinalAssignmentVM
    {
        public int ClassroomInstanceId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập hướng dẫn.")]
        [Display(Name = "Hướng dẫn")]
        public string Instructions { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hạn nộp.")]
        [Display(Name = "Hạn nộp")]
        public string DueDate { get; set; }

        // 🔹 Thêm thông tin để hiển thị header
        [BindNever]
        public ClassroomInstance Instance { get; set; }
        [BindNever]
        public ClassroomTemplate Template { get; set; }
        [BindNever]
        public string PartnerName { get; set; }
    }
}

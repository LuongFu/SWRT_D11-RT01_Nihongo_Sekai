using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearningPlatform.Models
{
    public class Classroom
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; } // thumbnail

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsApproved { get; set; } = true;

        // Mối quan hệ với Partner
        public string PartnerId { get; set; }
        [ForeignKey("PartnerId")]
        public ApplicationUser Partner { get; set; }

        // Danh sách học viên đã đăng ký
        public List<ClassroomRegistration> Registrations { get; set; }
    }
}

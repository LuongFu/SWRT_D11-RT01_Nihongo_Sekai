using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class CourseSectionVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bắt buộc điền")]
        [Display(Name = "Tiêu đề mục")]
        public string Title { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}

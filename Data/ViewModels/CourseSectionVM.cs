using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class CourseSectionVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Section title is required")]
        [Display(Name = "Section Title")]
        public string Title { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}

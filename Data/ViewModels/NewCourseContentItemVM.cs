using JapaneseLearningPlatform.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class NewCourseContentItemVM
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "Content Type")]
        [Required]
        public ContentType ContentType { get; set; }

        // Video input fields
        [Display(Name = "Video URL")]
        public string? VideoURL { get; set; }

        [Display(Name = "Video Description")]
        public string? VideoDescription { get; set; }

        // Quiz association (if selected)
        [Display(Name = "Quiz")]
        public int? QuizId { get; set; }

        // Mapping identifiers
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        public int SectionId { get; set; }
        public string SectionTitle { get; set; } = string.Empty;

        // Optional display order (for future sorting)
        public int? DisplayOrder { get; set; }

        // Dropdowns (only for view)
        public IEnumerable<SelectListItem>? Quizzes { get; set; }
    }
}

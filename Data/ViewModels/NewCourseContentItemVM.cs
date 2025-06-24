using JapaneseLearningPlatform.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class NewCourseContentItemVM : IValidatableObject
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

        [Display(Name = "New Quiz Title")]
        public string? NewQuizTitle { get; set; }

        [Display(Name = "New Quiz Description")]
        public string? NewQuizDescription { get; set; }

        // Mapping identifiers
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        public int SectionId { get; set; }
        public string SectionTitle { get; set; } = string.Empty;

        // Optional display order (for future sorting)
        public int? DisplayOrder { get; set; }

        // Dropdowns (only for view)
        public IEnumerable<SelectListItem>? Quizzes { get; set; }

        // validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ContentType == ContentType.Video)
            {
                if (string.IsNullOrWhiteSpace(VideoURL))
                    yield return new ValidationResult("Video URL is required", new[] { nameof(VideoURL) });

                if (string.IsNullOrWhiteSpace(VideoDescription))
                    yield return new ValidationResult("Video description is required", new[] { nameof(VideoDescription) });
            }

            if (ContentType == ContentType.Quiz)
            {
                if (string.IsNullOrWhiteSpace(NewQuizTitle))
                    yield return new ValidationResult("Quiz title is required", new[] { nameof(NewQuizTitle) });
            }
        }
    }
}

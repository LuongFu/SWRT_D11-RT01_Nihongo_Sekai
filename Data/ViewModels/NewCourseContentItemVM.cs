using JapaneseLearningPlatform.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class NewCourseContentItemVM : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Loại nội dung")]
        [Required]
        public ContentType ContentType { get; set; }

        // Video input fields
        [Display(Name = "Video URL")]
        public string? VideoURL { get; set; }

        [Display(Name = "Mô tả Video")]
        public string? VideoDescription { get; set; }

        // Quiz association (if selected)
        [Display(Name = "Quiz")]
        public int? QuizId { get; set; }

        [Display(Name = "Tiêu đề Quiz mới")]
        public string? NewQuizTitle { get; set; }

        [Display(Name = "Mô tả Quiz mới")]
        public string? NewQuizDescription { get; set; }

        // Mapping identifiers
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        public int SectionId { get; set; }
        public string SectionTitle { get; set; } = string.Empty;

        // Optional display order (for future sorting)
        [Display(Name = "Thứ tự hiển thị")]
        public int? DisplayOrder { get; set; }

        // Dropdowns (only for view)
        public IEnumerable<SelectListItem>? Quizzes { get; set; }

        // validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ContentType == ContentType.Video)
            {
                if (string.IsNullOrWhiteSpace(VideoURL))
                    yield return new ValidationResult("Video URL là bắt buộc", new[] { nameof(VideoURL) });

                if (string.IsNullOrWhiteSpace(VideoDescription))
                    yield return new ValidationResult("Mô tả Video là bắt buộc", new[] { nameof(VideoDescription) });
            }

            if (ContentType == ContentType.Quiz)
            {
                if (string.IsNullOrWhiteSpace(NewQuizTitle))
                    yield return new ValidationResult("Tiêu đề Quiz là bắt buộc", new[] { nameof(NewQuizTitle) });
            }
        }
    }
}

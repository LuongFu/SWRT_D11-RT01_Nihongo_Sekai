using JapaneseLearningPlatform.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class Video : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "URL Video Link")]
        [Required(ErrorMessage = "URL Video Link là bắt buộc")]
        public string VideoURL { get; set; }

        [Display(Name = "Video mô tả")]
        [Required(ErrorMessage = "Video mô tả là bắt buộc")]
        public string? VideoDescription { get; set; }

        //Relationships
        public List<Video_Course> Videos_Courses { get; set; }
    }
}

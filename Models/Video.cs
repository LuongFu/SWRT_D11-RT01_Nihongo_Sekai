using JapaneseLearningPlatform.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Models
{
    public class Video : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "URL Video Link")]
        [Required(ErrorMessage = "URL Video Link is required")]
        public string VideoURL { get; set; }

        [Display(Name = "Video Description")]
        [Required(ErrorMessage = "Video Description is required")]
        public string? VideoDescription { get; set; }

        //Relationships
        public List<Video_Course> Videos_Courses { get; set; }
    }
}

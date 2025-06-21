using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Models
{
    public class Course:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CourseCategory CourseCategory { get; set; }

        //Relationships
        public List<Video_Course> Videos_Courses { get; set; }
        // Relationship: Course → CourseSection
        public List<CourseSection> Sections { get; set; }
    }
}

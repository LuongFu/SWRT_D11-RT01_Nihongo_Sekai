using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearningPlatform.Models
{
    public class Course:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? ImageURL { get; set; }
        public int? DiscountPercent { get; set; } // null = no discount
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public CourseCategory CourseCategory { get; set; }

        //Relationships
        public List<Video_Course>? Videos_Courses { get; set; }
        // Relationship: Course → CourseSection
        public List<CourseSection>? Sections { get; set; }
        // Computed Price
        [NotMapped]
        public double FinalPrice
        {
            get
            {
                if (DiscountPercent.HasValue
                    && StartDate.HasValue
                    && EndDate.HasValue
                    && DateTime.Now >= StartDate.Value
                    && DateTime.Now <= EndDate.Value)
                {
                    return Price * (100 - DiscountPercent.Value) / 100;
                }
                return Price;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using JapaneseLearningPlatform.Data.Enums;

namespace JapaneseLearningPlatform.Models
{
    public class ClassroomInstance
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public ClassroomTemplate Template { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan ClassTime { get; set; }
        public int MaxCapacity { get; set; } = 20; 
        public decimal Price { get; set; }
        public bool IsPaid => Price > 0;
        public string? GoogleMeetLink { get; set; }
        public ClassroomStatus Status { get; set; } = ClassroomStatus.Draft;
        public ICollection<ClassroomEnrollment> Enrollments { get; set; } = new List<ClassroomEnrollment>();
        public List<FinalAssessment>? Assessments { get; set; }
    }
}

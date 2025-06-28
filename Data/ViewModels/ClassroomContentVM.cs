using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomContentVM
    {
        public ClassroomInstance Instance { get; set; }
        public ClassroomTemplate Template { get; set; }
        public string? PartnerName { get; set; }
        public FinalAssessment? FinalAssessment { get; set; }
        public AssessmentSubmission? Submission { get; set; }
        public bool HasSubmitted { get; set; }
        public bool HasReviewed { get; set; }
    }

}

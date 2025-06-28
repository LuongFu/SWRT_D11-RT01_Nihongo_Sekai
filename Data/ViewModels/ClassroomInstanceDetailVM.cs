using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomInstanceDetailVM
    {
        public ClassroomInstance Instance { get; set; }
        public ClassroomTemplate Template { get; set; }
        public int EnrollmentCount { get; set; }
        public bool IsPaid { get; set; }
        public string? PartnerName { get; set; }
    }
}

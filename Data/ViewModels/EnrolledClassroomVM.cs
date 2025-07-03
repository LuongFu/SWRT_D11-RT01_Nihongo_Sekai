namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class EnrolledClassroomVM
    {
        public int EnrollmentId { get; set; }
        public int InstanceId { get; set; }
        public string ClassTitle { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasLeft { get; set; }
    }

}

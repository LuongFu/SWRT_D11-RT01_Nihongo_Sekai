namespace JapaneseLearningPlatform.Models
{
    public class ClassroomEnrollment
    {
        public int Id { get; set; }

        public int InstanceId { get; set; }
        public ClassroomInstance Instance { get; set; }

        public string LearnerId { get; set; }
        public ApplicationUser Learner { get; set; }

        public DateTime EnrolledAt { get; set; }
        public bool HasLeft { get; set; }                   // Học viên đã rút khỏi lớp chưa
        public bool IsPaid { get; set; }                    // Đã thanh toán hay chưa (nếu lớp tính phí)
    }

}

using JapaneseLearningPlatform.Models;

public class ClassroomRegistration
{
    public int Id { get; set; }

    public int ClassroomId { get; set; }
    public Classroom Classroom { get; set; }

    public string LearnerId { get; set; }
    public ApplicationUser Learner { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.Now;
}

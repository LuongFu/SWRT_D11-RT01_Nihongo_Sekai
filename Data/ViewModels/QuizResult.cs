using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class QuizResult
    {
        public int Id { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime SubmittedAt { get; set; }
        public int Score { get; set; } // số câu đúng
        public int TotalQuestions { get; set; }

        public ICollection<QuizResultDetail> Details { get; set; }
    }

}

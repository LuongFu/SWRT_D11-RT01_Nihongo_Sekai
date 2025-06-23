using JapaneseLearningPlatform.Data.ViewModels;

namespace JapaneseLearningPlatform.Data.Services
{
    public interface IQuizQuestionsService
    {
        Task<QuizQuestionFormVM?> GetQuestionFormAsync(int? questionId, int quizId);
        Task SaveQuestionAsync(QuizQuestionFormVM vm);
        Task DeleteQuestionAsync(int questionId);
    }

}

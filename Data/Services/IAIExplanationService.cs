namespace JapaneseLearningPlatform.Data.Services
{
    public interface IAIExplanationService
    {
        Task<string> GetExplanationAsync(string question, string userAnswer, string correctAnswer);
    }
}

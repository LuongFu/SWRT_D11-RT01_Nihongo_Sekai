namespace JapaneseLearningPlatform.Data.Services
{
    public interface ICourseRatingService
    {
        Task AddRatingAsync(string userId, int courseId, int stars, string? comment);
        Task<(double avg, int total, Dictionary<int, int> counts)> GetStatsAsync(int courseId);
        Task<List<CourseRating>> GetLatestAsync(int courseId, int pageSize, int page, string sort);
    }
}
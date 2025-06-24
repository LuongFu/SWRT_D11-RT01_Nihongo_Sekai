namespace JapaneseLearningPlatform.Data.Services
{

    public interface ICourseContentItemsService
    {
        Task<IEnumerable<CourseContentItem>> GetBySectionIdAsync(int sectionId);
        Task<CourseContentItem> GetByIdAsync(int id);
        Task AddAsync(CourseContentItem item);
        Task UpdateAsync(CourseContentItem item);
        Task DeleteAsync(int id);
    }

}

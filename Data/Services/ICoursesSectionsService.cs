namespace JapaneseLearningPlatform.Data.Services
{
    public interface ICourseSectionsService
    {
        Task<IEnumerable<CourseSection>> GetSectionsByCourseIdAsync(int courseId);
        Task<CourseSection> GetByIdAsync(int id);
        Task AddAsync(CourseSection section);
        Task UpdateAsync(CourseSection section);
        Task DeleteAsync(int id);
    }

}

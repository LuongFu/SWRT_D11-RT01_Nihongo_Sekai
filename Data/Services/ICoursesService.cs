using JapaneseLearningPlatform.Data.Base;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.Services
{
    public interface ICoursesService : IEntityBaseRepository<Course>
    {
        Task<Course> GetCourseByIdAsync(int id);
        Task<NewCourseDropdownsVM> GetNewCourseDropdownsValues();
        Task AddNewCourseAsync(NewCourseVM data);
        Task UpdateCourseAsync(NewCourseVM data);
        Task<CourseHierarchyVM> GetCourseHierarchyAsync(int courseId, string userId, string cartId);
        Task<IEnumerable<CourseListItemVM>> GetFeaturedCoursesAsync();
        Task<List<CourseWithPurchaseVM>> GetAllCoursesWithPurchaseInfoAsync(string userId, string shoppingCartId);
        Task<IEnumerable<CourseListItemVM>> GetRecommendedCoursesAsync(string userId, int limit = 4);

    }
}

using JapaneseLearningPlatform.Data.Base;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using NihongoSekaiPlatform.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Data.Services
{
    public interface ICoursesService : IEntityBaseRepository<Course>
    {
        Task<Course> GetCourseByIdAsync(int id);
        Task<NewCourseDropdownsVM> GetNewCourseDropdownsValues();
        Task AddNewCourseAsync(NewCourseVM data);
        Task UpdateCourseAsync(NewCourseVM data);
        Task<CourseHierarchyVM> GetCourseHierarchyAsync(int courseId, string userId, string cartId);

    }
}

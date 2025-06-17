using NihongoSekaiWebApplication_D11_RT01.Data.Base;
using NihongoSekaiWebApplication_D11_RT01.Data.ViewModels;
using NihongoSekaiWebApplication_D11_RT01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NihongoSekaiWebApplication_D11_RT01.Data.Services
{
    public interface ICoursesService : IEntityBaseRepository<Course>
    {
        Task<Course> GetCourseByIdAsync(int id);
        Task<NewCourseDropdownsVM> GetNewCourseDropdownsValues();
        Task AddNewCourseAsync(NewCourseVM data);
        Task UpdateCourseAsync(NewCourseVM data);
    }
}

using JapaneseLearningPlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Data.Services
{
    public interface IClassroomTemplateService
    {
        Task<IEnumerable<ClassroomTemplate>> GetAllAsync();
        Task<ClassroomTemplate?> GetByIdAsync(int id);
        Task AddAsync(ClassroomTemplate template);
        Task UpdateAsync(ClassroomTemplate template);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

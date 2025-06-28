using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.Services
{
    public interface IClassroomInstanceService
    {
        Task<IEnumerable<ClassroomInstance>> GetAllAsync();
        Task<ClassroomInstance?> GetByIdAsync(int id);
        Task AddAsync(ClassroomInstance classroomInstance);
        Task UpdateAsync(ClassroomInstance classroomInstance);
        Task DeleteAsync(int id);

        Task<IEnumerable<ClassroomInstance>> GetByTemplateIdAsync(int templateId);
        Task<IEnumerable<ClassroomInstance>> GetByPartnerIdAsync(string partnerId);
    }
}

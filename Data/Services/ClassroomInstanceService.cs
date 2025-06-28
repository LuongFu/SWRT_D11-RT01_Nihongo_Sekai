using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data.Services
{
    public class ClassroomInstanceService : IClassroomInstanceService
    {
        private readonly AppDbContext _context;

        public ClassroomInstanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassroomInstance>> GetAllAsync()
        {
            return await _context.ClassroomInstances
                .Include(ci => ci.Template)
                .Include(ci => ci.Enrollments)
                    .ThenInclude(e => e.Learner)
                .ToListAsync();
        }

        public async Task<ClassroomInstance?> GetByIdAsync(int id)
        {
            return await _context.ClassroomInstances
                .Include(ci => ci.Template)
                .Include(ci => ci.Enrollments)
                    .ThenInclude(e => e.Learner)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task AddAsync(ClassroomInstance classroomInstance)
        {
            await _context.ClassroomInstances.AddAsync(classroomInstance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClassroomInstance classroomInstance)
        {
            _context.ClassroomInstances.Update(classroomInstance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var classroom = await _context.ClassroomInstances.FindAsync(id);
            if (classroom != null)
            {
                _context.ClassroomInstances.Remove(classroom);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ClassroomInstance>> GetByTemplateIdAsync(int templateId)
        {
            return await _context.ClassroomInstances
                .Where(ci => ci.TemplateId == templateId)
                .Include(ci => ci.Template)
                .Include(ci => ci.Enrollments)
                .ToListAsync();
        }

        public async Task<IEnumerable<ClassroomInstance>> GetByPartnerIdAsync(string partnerId)
        {
            return await _context.ClassroomInstances
                .Include(ci => ci.Template)
                .Include(ci => ci.Enrollments)
                .Where(ci => ci.Template.PartnerId == partnerId)
                .ToListAsync();
        }
    }
}

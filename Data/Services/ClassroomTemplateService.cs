using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Data.Services
{
    public class ClassroomTemplateService : IClassroomTemplateService
    {
        private readonly AppDbContext _context;

        public ClassroomTemplateService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassroomTemplate>> GetAllAsync()
        {
            return await _context.ClassroomTemplates.ToListAsync();
        }

        public async Task<ClassroomTemplate?> GetByIdAsync(int id)
        {
            return await _context.ClassroomTemplates.FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task AddAsync(ClassroomTemplate template)
        {
            await _context.ClassroomTemplates.AddAsync(template);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClassroomTemplate template)
        {
            _context.ClassroomTemplates.Update(template);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var template = await _context.ClassroomTemplates.FindAsync(id);
            if (template != null)
            {
                _context.ClassroomTemplates.Remove(template);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ClassroomTemplates.AnyAsync(c => c.Id == id);
        }
    }
}

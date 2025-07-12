using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data.Services
{
    public class ClassroomTemplateService : IClassroomTemplateService
    {
        private readonly AppDbContext _context;

        public ClassroomTemplateService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ClassroomTemplate template)
        {
            _context.ClassroomTemplates.Add(template);
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
            return await _context.ClassroomTemplates.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<ClassroomTemplate>> GetAllAsync()
        {
            return await _context.ClassroomTemplates
                .Include(t => t.Partner)
                .ToListAsync();
        }

        public async Task<ClassroomTemplate?> GetByIdAsync(int id)
        {
            return await _context.ClassroomTemplates.FindAsync(id);
        }

        public async Task<ClassroomTemplate?> GetByIdWithPartnerAsync(int id)
        {
            return await _context.ClassroomTemplates
                .Include(t => t.Partner)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<ClassroomTemplate>> GetByPartnerIdAsync(string partnerId)
        {
            return await _context.ClassroomTemplates
                .Where(t => t.PartnerId == partnerId)
                .ToListAsync();
        }

        public async Task UpdateAsync(ClassroomTemplate template)
        {
            _context.ClassroomTemplates.Update(template);
            await _context.SaveChangesAsync();
        }
    }
}

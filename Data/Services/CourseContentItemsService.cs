using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CourseContentItemsService : ICourseContentItemsService
{
    private readonly AppDbContext _context;

    public CourseContentItemsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseContentItem>> GetBySectionIdAsync(int sectionId)
    {
        return await _context.CourseContentItems
            .Include(ci => ci.Video)
            .Include(ci => ci.Quiz)
            .Where(ci => ci.SectionId == sectionId)
            .ToListAsync();
    }

    public async Task<CourseContentItem> GetByIdAsync(int id)
    {
        return await _context.CourseContentItems
            .Include(ci => ci.Video)
            .Include(ci => ci.Quiz)
            .FirstOrDefaultAsync(ci => ci.Id == id);
    }

    public async Task AddAsync(CourseContentItem item)
    {
        await _context.CourseContentItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CourseContentItem item)
    {
        _context.CourseContentItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.CourseContentItems.FindAsync(id);
        if (item != null)
        {
            _context.CourseContentItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}

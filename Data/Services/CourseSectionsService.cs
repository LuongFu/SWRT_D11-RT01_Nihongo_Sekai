using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Services;
using Microsoft.EntityFrameworkCore;

public class CourseSectionsService : ICourseSectionsService
{
    private readonly AppDbContext _context;

    public CourseSectionsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseSection>> GetSectionsByCourseIdAsync(int courseId)
    {
        return await _context.CourseSections
            .Include(s => s.ContentItems)
            .Where(s => s.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<CourseSection> GetByIdAsync(int id)
    {
        return await _context.CourseSections
            .Include(s => s.ContentItems)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(CourseSection section)
    {
        await _context.CourseSections.AddAsync(section);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CourseSection section)
    {
        _context.CourseSections.Update(section);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var section = await _context.CourseSections
            .Include(s => s.ContentItems)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (section != null)
        {
            _context.CourseSections.Remove(section);
            await _context.SaveChangesAsync();
        }
    }
}

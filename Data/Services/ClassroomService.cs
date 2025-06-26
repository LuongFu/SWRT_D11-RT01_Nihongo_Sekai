//using JapaneseLearningPlatform.Data;
//using JapaneseLearningPlatform.Data.Services;
//using Microsoft.EntityFrameworkCore;
//using JapaneseLearningPlatform.Data.ViewModels;
//using JapaneseLearningPlatform.Models;

//public class ClassroomService : IClassroomService
//{
//    private readonly AppDbContext _context;

//    public ClassroomService(AppDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<IEnumerable<ClassroomListItemVM>> GetPopularClassroomsAsync(int limit)
//    {
//        var classrooms = await _context.Classrooms
//            .Where(c => c.IsApproved)
//            .OrderByDescending(c => c.Registrations.Count)
//            .Take(limit)
//            .Select(c => new ClassroomListItemVM
//            {
//                ClassroomId = c.Id,
//                Title = c.Title,
//                Description = c.Description,
//                Thumbnail = c.ImageUrl,
//                Schedule = $"{c.StartTime:ddd, HH:mm} - {c.EndTime:HH:mm}",
//                PartnerName = c.Partner.FullName
//            })
//            .ToListAsync();

//        return classrooms;
//    }
//}

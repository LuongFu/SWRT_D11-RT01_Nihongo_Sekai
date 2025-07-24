using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Services;  // ICertificateService
using JapaneseLearningPlatform.Data.ViewModels; // AchievementItemVM
using JapaneseLearningPlatform.Models; // CourseCertificate
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize]
    public class AchievementsController : Controller
    {
        private readonly AppDbContext _context;
        public AchievementsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Achievements
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. Lấy về toàn bộ khoá kèm sections + contentItems
            var allCourses = await _context.Courses
                .Include(c => c.Sections)
                   .ThenInclude(s => s.ContentItems)
                .ToListAsync();

            // 2. Lọc ra những khoá đã hoàn thành
            var completedCourses = allCourses
                .Where(course =>
                {
                    int totalItems = course.Sections.Sum(s => s.ContentItems.Count);
                    if (totalItems == 0) return false;    // không xét khoá trống
                                                          // đếm số content đã completed
                    int doneCount = _context.CourseContentProgresses
                        .Count(p => p.UserId == userId
                                 && p.CourseId == course.Id
                                 && p.IsCompleted);
                    return doneCount == totalItems;
                })
                .ToList();

            // 3. Map sang VM để đổ ra view
            var vm = completedCourses.Select(c => new AchievementItemVM
            {
                CourseId = c.Id,
                CourseName = c.Name,
                ThumbnailUrl = c.ImageURL,
                // Lấy thời điểm hoàn thành gần nhất
                CompletedAt = _context.CourseContentProgresses
                    .Where(p => p.UserId == userId && p.CourseId == c.Id)
                    .Max(p => p.CompletedAt),
                FileUrl = ""    // nếu bạn vẫn muốn giữ file URL của certificate (nếu đã có)
            }).ToList();

            if (!vm.Any())
                ViewBag.Message = "Bạn chưa hoàn thành khoá nào.";
            return View(vm);
        }
    }

}

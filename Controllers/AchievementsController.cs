using JapaneseLearningPlatform.Data.Services;  // ICertificateService
using JapaneseLearningPlatform.Data.ViewModels; // AchievementItemVM
using JapaneseLearningPlatform.Models; // CourseCertificate
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JapaneseLearningPlatform.Data;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize]
    public class AchievementsController : Controller
    {
        private readonly ICertificateService _certService;
        private readonly AppDbContext _context;
        public AchievementsController(ICertificateService certService, AppDbContext context)
        {
            _certService = certService;
            _context = context;
        }

        // GET: /Achievements
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var certs = await _certService.GetByUserAsync(userId);

            // Nếu chưa có chứng chỉ, hiển thị thông báo
            if (!certs.Any())
            {
                ViewBag.Message = "Bạn chưa hoàn thành khóa nào, vì vậy chưa có chứng chỉ.";
            }

            // Map CourseCertificate → AchievementItemVM nếu cần
            var vm = certs.Select(c => new AchievementItemVM
            {
                CourseId = c.CourseId,
                CourseName = c.Course.Name,
                ThumbnailUrl = c.Course.ImageURL,
                CompletedAt = c.IssuedAt,
                FileUrl = c.FileUrl
            }).ToList();
            return View(vm);
        }


        // GET: /Achievements/Certificate/5
        public async Task<IActionResult> Certificate(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if user has a certificate for the given course
            var cert = await _certService.GetByUserAsync(userId)
                         .ContinueWith(t => t.Result
                           .FirstOrDefault(c => c.CourseId == courseId));

            if (cert == null)
            {
                return NotFound("Certificate not found");
            }

            // Redirect to certificate URL if found
            return Redirect(cert.FileUrl);
        }

        // POST: /Achievements/MarkAsCompleted
        // You can add a method here if you want to explicitly mark courses as completed and stored in achievements.
        // This can be done automatically when the course reaches 100% progress.
    }
}

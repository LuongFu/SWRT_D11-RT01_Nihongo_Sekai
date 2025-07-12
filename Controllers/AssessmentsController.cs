using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace JapaneseLearningPlatform.Controllers
{
    public class AssessmentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AssessmentsController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [Authorize(Roles = "Learner")]
        [HttpPost]
        public async Task<IActionResult> SubmitAssessment(int instanceId, string? answerText, IFormFile? SubmissionFile)
        {
            var user = await _userManager.GetUserAsync(User);

            // 🔍 Tìm FinalAssessment theo ClassroomInstanceId
            var assessment = await _context.FinalAssessments
                .FirstOrDefaultAsync(a => a.ClassroomInstanceId == instanceId);

            if (assessment == null)
            {
                return NotFound("Assessment not found for this class.");
            }

            if (assessment.DueDate != null && DateTime.Now > assessment.DueDate)
            {
                return BadRequest("The assessment deadline has passed.");
            }

            if (string.IsNullOrWhiteSpace(answerText) && (SubmissionFile == null || SubmissionFile.Length == 0))
            {
                TempData["Message"] = "❌ Please enter an answer or upload a file before submitting.";
                return Redirect($"/ClassroomInstances/Content/{instanceId}#assessment");
            }

            string? filePath = null;

            if (SubmissionFile != null && SubmissionFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "submissions");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}_{SubmissionFile.FileName}";
                var path = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await SubmissionFile.CopyToAsync(stream);
                }

                filePath = "/submissions/" + fileName;
            }

            var existing = await _context.AssessmentSubmissions
                .FirstOrDefaultAsync(s => s.LearnerId == user.Id && s.FinalAssessmentId == assessment.Id);

            if (existing != null)
            {
                // ⚠️ Nếu đã được chấm điểm thì không cho sửa
                if (existing.Score != null)
                {
                    TempData["Message"] = "❌ You cannot update your submission after it has been graded.";
                    return Redirect($"/ClassroomInstances/Content/{instanceId}#assessment");
                }

                // Xóa file cũ nếu có và có file mới upload
                if (!string.IsNullOrEmpty(existing.FilePath) && filePath != null)
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, existing.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                //Lưu nội dung mới
                existing.FilePath = filePath ?? existing.FilePath;
                existing.AnswerText = answerText;
                existing.SubmittedAt = DateTime.Now;
            }
            else
            {
                var submission = new AssessmentSubmission
                {
                    FinalAssessmentId = assessment.Id,
                    LearnerId = user.Id,
                    FilePath = filePath,
                    AnswerText = answerText, // ✅ thêm dòng này
                    SubmittedAt = DateTime.Now
                };

                _context.AssessmentSubmissions.Add(submission);
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Submitted successfully!";

            return Redirect($"/ClassroomInstances/Content/{instanceId}#assessment");
        }

        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> Grade(int submissionId)
        {
            var submission = await _context.AssessmentSubmissions
                .Include(s => s.Learner)
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.Instance)
                        .ThenInclude(i => i.Template)
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var isOwnerPartner = submission.Assessment.Instance.Template.PartnerId == userId;

            if (!isOwnerPartner) return Forbid();

            var instance = submission.Assessment.Instance;
            var vm = new GradeSubmissionVM
            {
                SubmissionId = submission.Id,
                LearnerName = submission.Learner?.FullName,
                AnswerText = submission.AnswerText,
                FilePath = submission.FilePath,
                Score = submission.Score,
                Feedback = submission.Feedback,
                InstanceId = submission.Assessment.ClassroomInstanceId,

                // Thêm các trường header lớp
                ClassTitle = instance.Template.Title,
                PartnerName = instance.Template.Partner?.FullName,
                StartDate = instance.StartDate,
                EndDate = instance.EndDate,
                ClassTime = instance.ClassTime,
                GoogleMeetLink = instance.GoogleMeetLink
            };

            return View("~/Views/ClassroomInstances/Grade.cshtml", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> Grade(GradeSubmissionVM vm)
        {
            var submission = await _context.AssessmentSubmissions
                .Include(s => s.Assessment)
                    .ThenInclude(a => a.Instance)
                        .ThenInclude(i => i.Template) // ✅ BỔ SUNG dòng này
                .FirstOrDefaultAsync(s => s.Id == vm.SubmissionId);

            if (submission == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var isOwnerPartner = submission.Assessment.Instance.Template.PartnerId == userId;
            if (!isOwnerPartner) return Forbid();

            submission.Score = vm.Score;
            submission.Feedback = vm.Feedback;

            await _context.SaveChangesAsync();
            TempData["Message"] = "Graded successfully!";

            return Redirect($"/ClassroomInstances/Content/{vm.InstanceId}#assessment");
        }

        [HttpPost]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> UpdateDeadline(int assessmentId, string newDueDate)
        {
            var assessment = await _context.FinalAssessments
                .Include(a => a.Instance)
                    .ThenInclude(i => i.Template)
                .FirstOrDefaultAsync(a => a.Id == assessmentId);

            if (assessment == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (assessment.Instance.Template.PartnerId != userId)
                return Forbid();

            if (!DateTime.TryParseExact(newDueDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDueDate))
            {
                TempData["DeadlineMessage"] = "❌ Invalid date format.";
                return Redirect($"/ClassroomInstances/Content/{assessment.ClassroomInstanceId}#assessment");
            }

            //kiểm tra tính tương lai của deadline mới
            if (parsedDueDate <= assessment.DueDate)
            {
                TempData["DeadlineMessage"] = "❌ New deadline must be later than the current one.";
                return Redirect($"/ClassroomInstances/Content/{assessment.ClassroomInstanceId}#assessment");
            }

            assessment.DueDate = parsedDueDate;
            await _context.SaveChangesAsync();

            TempData["DeadlineMessage"] = "✅ Deadline updated successfully.";
            return Redirect($"/ClassroomInstances/Content/{assessment.ClassroomInstanceId}#assessment");
        }

    }
}

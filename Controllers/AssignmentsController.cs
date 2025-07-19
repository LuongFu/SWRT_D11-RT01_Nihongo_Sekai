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
    public class AssignmentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AssignmentsController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [Authorize(Roles = "Learner")]
        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(int instanceId, string? answerText, IFormFile? SubmissionFile)
        {
            var user = await _userManager.GetUserAsync(User);

            // 🔍 Tìm FinalAssignment theo ClassroomInstanceId
            var assignment = await _context.FinalAssignments
                .FirstOrDefaultAsync(a => a.ClassroomInstanceId == instanceId);

            if (assignment == null)
            {
                return NotFound("Không tìm thấy bài tập ở lớp này.");
            }

            if (assignment.DueDate != null && DateTime.Now > assignment.DueDate)
            {
                return BadRequest("Hạn cuối của bài tập đã qua.");
            }

            if (string.IsNullOrWhiteSpace(answerText) && (SubmissionFile == null || SubmissionFile.Length == 0))
            {
                TempData["Message"] = "❌ Vui lòng nhập câu trả lời hoặc tải file bài làm trước khi nộp.";
                return Redirect($"/ClassroomInstances/Content/{instanceId}#assignment");
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

            var existing = await _context.AssignmentSubmissions
                .FirstOrDefaultAsync(s => s.LearnerId == user.Id && s.FinalAssignmentId == assignment.Id);

            if (existing != null)
            {
                // ⚠️ Nếu đã được chấm điểm thì không cho sửa
                if (existing.Score != null)
                {
                    TempData["Message"] = "❌ Bạn không thể chỉnh sửa bài làm sau khi đã được chấm.";
                    return Redirect($"/ClassroomInstances/Content/{instanceId}#assignment");
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
                var submission = new AssignmentSubmission
                {
                    FinalAssignmentId = assignment.Id,
                    LearnerId = user.Id,
                    FilePath = filePath,
                    AnswerText = answerText, // ✅ thêm dòng này
                    SubmittedAt = DateTime.Now
                };

                _context.AssignmentSubmissions.Add(submission);
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Gửi bài làm thành công!";

            return Redirect($"/ClassroomInstances/Content/{instanceId}#assignment");
        }

        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> Grade(int submissionId)
        {
            var submission = await _context.AssignmentSubmissions
                .Include(s => s.Learner)
                .Include(s => s.Assignment)
                    .ThenInclude(a => a.Instance)
                        .ThenInclude(i => i.Template)
                              .ThenInclude(t => t.Partner) // 👈 Bổ sung Include Partner
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var isOwnerPartner = submission.Assignment.Instance.Template.PartnerId == userId;

            if (!isOwnerPartner) return Forbid();

            var instance = submission.Assignment.Instance;
            var vm = new GradeSubmissionVM
            {
                SubmissionId = submission.Id,
                LearnerName = submission.Learner?.FullName,
                AnswerText = submission.AnswerText,
                FilePath = submission.FilePath,
                Score = submission.Score,
                Feedback = submission.Feedback,
                InstanceId = submission.Assignment.ClassroomInstanceId,

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
            var submission = await _context.AssignmentSubmissions
                .Include(s => s.Assignment)
                    .ThenInclude(a => a.Instance)
                        .ThenInclude(i => i.Template) // ✅ BỔ SUNG dòng này
                .FirstOrDefaultAsync(s => s.Id == vm.SubmissionId);

            if (submission == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var isOwnerPartner = submission.Assignment.Instance.Template.PartnerId == userId;
            if (!isOwnerPartner) return Forbid();

            submission.Score = vm.Score;
            submission.Feedback = vm.Feedback;

            await _context.SaveChangesAsync();
            TempData["Message"] = "Chấm điểm thành công!";

            return Redirect($"/ClassroomInstances/Content/{vm.InstanceId}#assignment");
        }

        [HttpGet]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> Create(int instanceId)
        {
            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                    .ThenInclude(t => t.Partner)
                .FirstOrDefaultAsync(i => i.Id == instanceId);

            if (instance == null)
                return NotFound();

            var vm = new FinalAssignmentVM
            {
                ClassroomInstanceId = instanceId,
                DueDate = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy HH:mm:ss"),
                Instance = instance,
                Template = instance.Template,
                PartnerName = instance.Template.Partner?.FullName ?? "N/A"
            };

            return View("~/Views/ClassroomInstances/CreateAssignment.cshtml", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> Create(FinalAssignmentVM vm)
        {
            ModelState.Remove("Instance");
            ModelState.Remove("Template");
            ModelState.Remove("PartnerName");

            if (!ModelState.IsValid)
            {
                await LoadInstanceData(vm);
                return View("~/Views/ClassroomInstances/CreateAssignment.cshtml", vm);
            }

            if (!DateTime.TryParseExact(vm.DueDate, "dd/MM/yyyy HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDueDate))
            {
                ModelState.AddModelError("DueDate", "Định dạng thời gian không hợp lệ.");
                await LoadInstanceData(vm);
                return View("~/Views/ClassroomInstances/CreateAssignment.cshtml", vm);
            }

            var finalAssignment = new FinalAssignment
            {
                ClassroomInstanceId = vm.ClassroomInstanceId,
                Instructions = vm.Instructions,
                DueDate = parsedDueDate
            };

            _context.FinalAssignments.Add(finalAssignment);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Bài tập cuối khóa đã được thêm thành công.";
            return Redirect(Url.Action("Content", "ClassroomInstances", new { id = vm.ClassroomInstanceId }) + "#assignment");
        }

        private async Task LoadInstanceData(FinalAssignmentVM vm)
        {
            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                    .ThenInclude(t => t.Partner)
                .FirstOrDefaultAsync(i => i.Id == vm.ClassroomInstanceId);

            if (instance != null)
            {
                vm.Instance = instance;
                vm.Template = instance.Template;
                vm.PartnerName = instance.Template.Partner?.FullName ?? "N/A";
            }
        }
        //DELETE ASSIGNMENT ONLY FOR DEBUG
        [Authorize(Roles = "Partner")]
        [HttpPost]
        public async Task<IActionResult> Delete(int assignmentId)
        {
            var assignment = await _context.FinalAssignments
                .Include(a => a.Instance)
                .ThenInclude(i => i.Template)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (assignment.Instance.Template.PartnerId != userId)
                return Forbid();

            // Xóa tất cả submissions của bài tập này (nếu có)
            var submissions = _context.AssignmentSubmissions
                .Where(s => s.FinalAssignmentId == assignment.Id);
            _context.AssignmentSubmissions.RemoveRange(submissions);

            _context.FinalAssignments.Remove(assignment);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Bài tập cuối khóa đã được xóa.";
            return Redirect($"/ClassroomInstances/Content/{assignment.ClassroomInstanceId}#assignment");
        }
    }
}

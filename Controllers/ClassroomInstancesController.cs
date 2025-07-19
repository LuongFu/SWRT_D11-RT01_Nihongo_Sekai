using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Helpers;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Controllers
{
    public class ClassroomInstancesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ClassroomInstancesController> _logger;

        public ClassroomInstancesController(AppDbContext context, UserManager<ApplicationUser> userManager, ILogger<ClassroomInstancesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
        {
            var userId = _userManager.GetUserId(User);

            var query = _context.ClassroomInstances
                .Include(i => i.Template)
                .Include(i => i.Enrollments)
                .Where(i => i.Status == ClassroomStatus.Published)
                .OrderByDescending(i => i.StartDate)
                .AsQueryable();

            int total = await query.CountAsync();
            var sessions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var vmList = sessions.Select(i =>
            {
                var vm = ClassroomInstanceMapper.ToVM(i);
                vm.IsEnrolled = userId != null && i.Enrollments.Any(e => e.LearnerId == userId && !e.HasLeft);
                return vm;
            }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            return View(vmList);
        }

        // PARTNER: My sessions
        [Authorize(Roles = UserRoles.Partner)]
        public async Task<IActionResult> MySession(int page = 1)
        {
            var userId = _userManager.GetUserId(User);
            var query = _context.ClassroomInstances
                .Include(i => i.Template)
                .Where(i => i.Template.PartnerId == userId)
                .OrderByDescending(i => i.StartDate);

            int total = await query.CountAsync();
            var sessions = await query
                .Skip((page - 1) * 6)
                .Take(6)
                .ToListAsync();

            var vmList = sessions.Select(ClassroomInstanceMapper.ToVM).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / 6.0);
            return View(vmList); // ✅ Dùng IEnumerable<ClassroomInstanceVM>
        }


        // PARTNER: GET Create session form
        [HttpGet]
        [Authorize(Roles = UserRoles.Partner)]
        public async Task<IActionResult> CreateSession(int? templateId)
        {
            var userId = _userManager.GetUserId(User);
            var templates = await _context.ClassroomTemplates
                .Where(t => t.PartnerId == userId)
                .ToListAsync();

            ViewBag.TemplateList = new SelectList(templates, "Id", "Title", templateId);

            var vm = new ClassroomInstanceVM();

            if (templateId.HasValue)
            {
                vm.TemplateId = templateId.Value;
                await PopulateTemplateData(vm); // ✅ Load info từ template được chọn
            }

            return View(vm);
        }


        // PARTNER: POST Create session
        [Authorize(Roles = UserRoles.Partner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSession(ClassroomInstanceVM vm)
        {
            var userId = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                // Ghi log lỗi cho từng trường sai
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        _logger.LogWarning("Validation error at {Key}: {Error}", key, error.ErrorMessage);
                    }
                }

                var templates = await _context.ClassroomTemplates
                    .Where(t => t.PartnerId == userId)
                    .ToListAsync();

                ViewBag.TemplateList = new SelectList(templates, "Id", "Title", vm.TemplateId);

                await PopulateTemplateData(vm);
                return View(vm);
            }

            var instance = new ClassroomInstance
            {
                TemplateId = vm.TemplateId,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                ClassTime = TimeSpan.FromHours(vm.SessionDurationHours),
                Price = vm.Price,
                GoogleMeetLink = vm.GoogleMeetLink,
                Status = vm.Status
            };

            _context.ClassroomInstances.Add(instance);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New classroom session created: TemplateId={TemplateId}, Start={StartDate}, End={EndDate}", vm.TemplateId, vm.StartDate, vm.EndDate);

            return RedirectToAction(nameof(MySession));
        }


        // PARTNER: Edit existing session
        // GET: EditSession (Partner chỉnh sửa session)
        [Authorize(Roles = UserRoles.Partner)]
        public async Task<IActionResult> EditSession(int id)
        {
            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instance == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (instance.Template.PartnerId != userId) return Forbid();

            // ✅ Gửi danh sách Status để hiển thị trong dropdown
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(ClassroomStatus))
                .Cast<ClassroomStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString()
                }), "Value", "Text", (int)instance.Status);

            return View(instance.ToVM());
        }

        // POST: EditSession
        [HttpPost]
        [Authorize(Roles = UserRoles.Partner)]
        public async Task<IActionResult> EditSession(int id, ClassroomInstanceVM vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(ClassroomStatus))
                    .Cast<ClassroomStatus>()
                    .Select(s => new SelectListItem
                    {
                        Value = ((int)s).ToString(),
                        Text = s.ToString()
                    }), "Value", "Text", (int)vm.Status);
                return View(vm);
            }

            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instance == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (instance.Template.PartnerId != userId) return Forbid();

            instance.StartDate = vm.StartDate;
            instance.EndDate = vm.EndDate;
            instance.Price = vm.Price;
            instance.GoogleMeetLink = vm.GoogleMeetLink;
            instance.ClassTime = TimeSpan.FromHours(vm.SessionDurationHours);
            instance.Status = vm.Status; // ✅ Update status từ form

            _context.Update(instance);
            await _context.SaveChangesAsync();

            return RedirectToAction("MySession");
        }

        // PARTNER: Delete session
        [Authorize(Roles = UserRoles.Partner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (instance == null) return NotFound();
            if (instance.Template.PartnerId != _userManager.GetUserId(User)) return Forbid();

            _context.ClassroomInstances.Remove(instance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MySession));
        }

        // PUBLIC: View session details & enroll
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                    .ThenInclude(t => t.Partner)
                .Include(i => i.Enrollments)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (instance == null) return NotFound();
            var userId = _userManager.GetUserId(User); // 👈 Bổ sung dòng này
            var enrollment = instance.Enrollments.FirstOrDefault(e => e.LearnerId == userId && !e.HasLeft);
            var vm = new ClassroomInstanceDetailVM
            {
                Instance = instance,
                Template = instance.Template,
                EnrollmentCount = instance.Enrollments.Count,
                PartnerName = instance.Template.Partner.FullName,
                IsEnrolled = enrollment != null,
                HasPaid = enrollment?.IsPaid == true, // ✅ thêm dòng này
                    IsPaid = instance.IsPaid // ✅ Gán IsPaid từ instance
            };
            return View(vm);
        }

        // SUPPORT: Populate view model with template data
        private async Task PopulateTemplateData(ClassroomInstanceVM vm)
        {
            var template = await _context.ClassroomTemplates.FindAsync(vm.TemplateId);
            if (template != null)
            {
                vm.TemplateTitle = template.Title;
                vm.TemplateDescription = template.Description;
                vm.TemplateImageURL = template.ImageURL;
                vm.LanguageLevel = template.LanguageLevel.ToString();
                vm.DocumentURL = template.DocumentURL;
                //vm.SessionTime = template.SessionTime;
            }
        }

        // ADMIN + PARTNER ONLY
        [Authorize(Roles = "Admin,Partner")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instance == null) return NotFound();

            if (User.IsInRole("Partner") && instance.Template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            ViewBag.Templates = new SelectList(
                await _context.ClassroomTemplates
                    .Where(t => User.IsInRole("Admin") || t.PartnerId == _userManager.GetUserId(User))
                    .ToListAsync(),
                "Id", "Title", instance.TemplateId);

            return View(instance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Partner")]
        public async Task<IActionResult> Edit(int id, ClassroomInstance instance)
        {
            if (id != instance.Id) return NotFound();

            var existing = await _context.ClassroomInstances
                .Include(i => i.Template)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (existing == null) return NotFound();

            if (User.IsInRole("Partner") && existing.Template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.Templates = new SelectList(
                    await _context.ClassroomTemplates
                        .Where(t => User.IsInRole("Admin") || t.PartnerId == _userManager.GetUserId(User))
                        .ToListAsync(), "Id", "Title", instance.TemplateId);
                return View(instance);
            }

            _context.Entry(existing).CurrentValues.SetValues(instance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ADMIN + PARTNER ONLY
        [Authorize(Roles = "Admin,Partner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instance == null) return NotFound();

            if (User.IsInRole("Partner") && instance.Template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            return View(instance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Partner")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instance == null) return NotFound();

            if (User.IsInRole("Partner") && instance.Template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            _context.ClassroomInstances.Remove(instance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // View CLASSROOM CONTENT
        [Authorize(Roles = "Learner,Partner")]
        public async Task<IActionResult> Content(int id)
        {
            var instance = await _context.ClassroomInstances
                .AsSplitQuery()              // ⚡ Chia query để tránh JOIN khổng lồ
                .AsNoTracking()              // ⚡ Không cần tracking vì chỉ đọc dữ liệu
                .Include(c => c.Template)
                    .ThenInclude(t => t.Partner)
                .Include(c => c.Assignments!)
                    .ThenInclude(a => a.Submissions!)
                        .ThenInclude(s => s.Learner)
                .Include(c => c.Enrollments!)
                    .ThenInclude(e => e.Learner)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (instance == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isLearner = User.IsInRole(UserRoles.Learner);
            var isPartner = User.IsInRole(UserRoles.Partner);
            var isAdmin = User.IsInRole(UserRoles.Admin);

            var isEnrolled = instance.Enrollments.Any(e => e.LearnerId == userId && !e.HasLeft);
            var isOwnerPartner = isPartner && instance.Template.PartnerId == userId;

            if (isLearner && !isEnrolled)
                return Forbid();

            if (isPartner && !isOwnerPartner)
                return Forbid();

            var finalAssignment = instance.Assignments?.FirstOrDefault();

            AssignmentSubmission? submission = null;
            List<AssignmentSubmission>? allSubmissions = null;

            if (finalAssignment != null)
            {
                if (isLearner)
                {
                    submission = await _context.AssignmentSubmissions
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.FinalAssignmentId == finalAssignment.Id && s.LearnerId == userId);
                }

                if (isPartner && instance.Assignments.First().Submissions != null)
                {
                    allSubmissions = instance.Assignments.First().Submissions.ToList();
                }
            }

            var reviewed = await _context.ClassroomEvaluations
                .AsNoTracking()
                .AnyAsync(e => e.InstanceId == id && e.LearnerId == userId);

            var vm = new ClassroomContentVM
            {
                Instance = instance,
                Template = instance.Template,
                PartnerName = instance.Template.Partner?.FullName,
                FinalAssignment = finalAssignment,
                Submission = submission,
                HasSubmitted = submission != null,
                HasReviewed = reviewed,
                AllSubmissions = allSubmissions
            };

            return View(vm);
        }



        [Authorize(Roles = UserRoles.Learner)]
        public async Task<IActionResult> PayWithPaypal(int id)
        {
            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instance == null)
                return NotFound();

            // OPTIONAL: nếu bạn chỉ cảnh báo, không trả về lỗi 404
            if (!instance.IsPaid)
                return BadRequest("This classroom is not eligible for PayPal payment.");

            var vm = new ClassroomPaymentVM
            {
                InstanceId = instance.Id,
                Title = instance.Template?.Title,
                Price = instance.Price,
                Currency = "USD"
            };

            return View(vm);
        }


        [Authorize(Roles = UserRoles.Learner)]
        public async Task<IActionResult> CompletePayment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var instance = await _context.ClassroomInstances
                .Include(i => i.Enrollments)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instance == null) return NotFound();

            // Kiểm tra đã đăng ký chưa
            bool isEnrolled = instance.Enrollments.Any(e => e.LearnerId == userId);
            if (!isEnrolled)
            {
                _context.ClassroomEnrollments.Add(new ClassroomEnrollment
                {
                    LearnerId = userId,
                    InstanceId = id,
                    IsPaid = true,
                    EnrolledAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id });
        }
    }
}

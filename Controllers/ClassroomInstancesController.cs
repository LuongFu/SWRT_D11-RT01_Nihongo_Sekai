using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Controllers
{
    public class ClassroomInstancesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassroomInstancesController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // PUBLIC - Allow everyone
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
        {
            var query = _context.ClassroomInstances
                .Include(i => i.Template)
                .Include(i => i.Enrollments)
                .AsQueryable();

            var isPartner = User.IsInRole(UserRoles.Partner);
            var isAdmin = User.IsInRole(UserRoles.Admin);

            if (isPartner)
            {
                var userId = _userManager.GetUserId(User);
                query = query.Where(i => i.Template.PartnerId == userId);
            }
            else if (!isAdmin) // Guest hoặc Learner
            {
                query = query.Where(i => i.Status != ClassroomStatus.Draft);
            }

            int totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(i => i.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = items.Select(i => new ClassroomInstanceWithTemplateVM
            {
                Instance = i,
                Template = i.Template,
                EnrollmentCount = i.Enrollments?.Count ?? 0
            }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(model);
        }

        // PUBLIC - Learner or Guest allowed
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                    .ThenInclude(t => t.Partner)
                .Include(i => i.Enrollments)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instance == null) return NotFound();

            if (instance.Status == ClassroomStatus.Draft &&
                (!User.Identity.IsAuthenticated || User.IsInRole(UserRoles.Learner)))
            {
                return Forbid();
            }

            bool isEnrolled = false;

            if (User.Identity.IsAuthenticated && User.IsInRole(UserRoles.Learner))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                isEnrolled = instance.Enrollments.Any(e => e.LearnerId == userId && !e.HasLeft);
            }

            var vm = new ClassroomInstanceDetailVM
            {
                Instance = instance,
                Template = instance.Template,
                EnrollmentCount = instance.Enrollments?.Count ?? 0,
                IsPaid = instance.IsPaid,
                PartnerName = instance.Template?.Partner?.FullName,
                IsEnrolled = isEnrolled
            };

            return View(vm);
        }

        // ADMIN + PARTNER ONLY
        [Authorize(Roles = "Admin,Partner")]
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            var templates = await _context.ClassroomTemplates
                .Where(t => User.IsInRole("Admin") || t.PartnerId == userId)
                .ToListAsync();

            ViewBag.Templates = new SelectList(templates, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Partner")]
        public async Task<IActionResult> Create(ClassroomInstance instance)
        {
            var userId = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                ViewBag.Templates = new SelectList(_context.ClassroomTemplates
                    .Where(t => User.IsInRole("Admin") || t.PartnerId == userId), "Id", "Title", instance.TemplateId);
                return View(instance);
            }

            _context.Add(instance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        // LEARNER ONLY
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> Content(int id)
        {
            var instance = await _context.ClassroomInstances
                .Include(c => c.Template)
                .Include(c => c.Assessments)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (instance == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isEnrolled = instance.Enrollments.Any(e => e.LearnerId == userId && !e.HasLeft);
            if (!isEnrolled) return Forbid();

            var finalAssessment = instance.Assessments?.FirstOrDefault();
            AssessmentSubmission? submission = null;
            if (finalAssessment != null)
            {
                submission = await _context.AssessmentSubmissions
                    .FirstOrDefaultAsync(s => s.FinalAssessmentId == finalAssessment.Id && s.LearnerId == userId);
            }
            var reviewed = await _context.ClassroomEvaluations
     .AnyAsync(e => e.InstanceId == id && e.LearnerId == userId);
            var vm = new ClassroomContentVM
            {
                Instance = instance,
                Template = instance.Template,
                PartnerName = instance.Template.Partner?.FullName,
                FinalAssessment = finalAssessment,
                Submission = submission,
                HasSubmitted = submission != null,
                HasReviewed = reviewed
            };

            return View(vm);
        }
    }
}

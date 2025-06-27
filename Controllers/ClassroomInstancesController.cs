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
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = "Admin,Partner")]
    public class ClassroomInstancesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassroomInstancesController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ClassroomInstances
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


        // GET: ClassroomInstances/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var instance = await _context.ClassroomInstances
                .Include(i => i.Template)
                .Include(i => i.Enrollments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instance == null) return NotFound();

            var isPartner = User.IsInRole(UserRoles.Partner);
            var isAdmin = User.IsInRole(UserRoles.Admin);

            if (isPartner && instance.Template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            if (!isAdmin && !isPartner && instance.Status == ClassroomStatus.Draft)
                return NotFound(); // Guest/Learner không xem được lớp chưa công bố

            return View(instance);
        }


        // GET: ClassroomInstances/Create
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);

            var templates = await _context.ClassroomTemplates
                .Where(t => User.IsInRole("Admin") || t.PartnerId == userId)
                .ToListAsync();

            ViewBag.Templates = new SelectList(templates, "Id", "Title");

            return View();
        }

        // POST: ClassroomInstances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: ClassroomInstances/Edit/5
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

        // POST: ClassroomInstances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: ClassroomInstances/Delete/5
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

        // POST: ClassroomInstances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
    }
}


using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = "Admin,Partner")]
    public class ClassroomTemplatesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassroomTemplatesController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ClassroomTemplates
        public async Task<IActionResult> Index()
        {
            var templates = await _context.ClassroomTemplates
                .Include(t => t.Partner)
                .ToListAsync();
            return View(templates);
        }

        // GET: ClassroomTemplates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var template = await _context.ClassroomTemplates
                .Include(t => t.Partner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (template == null) return NotFound();

            if (User.IsInRole("Partner"))
            {
                var userId = _userManager.GetUserId(User);
                if (template.PartnerId != userId)
                    return Forbid();
            }

            return View(template);
        }

        // GET: ClassroomTemplates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClassroomTemplates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassroomTemplate template)
        {
            if (!ModelState.IsValid) return View(template);

            // Set PartnerId for Partner role
            if (User.IsInRole("Partner"))
            {
                var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
                if (userId != null)
                    template.PartnerId = userId;
            }

            _context.Add(template);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: ClassroomTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var template = await _context.ClassroomTemplates.FindAsync(id);
            if (template == null) return NotFound();

            if (User.IsInRole("Partner"))
            {
                var userId = _userManager.GetUserId(User);
                if (template.PartnerId != userId)
                    return Forbid();
            }

            return View(template);
        }
        // POST: ClassroomTemplates/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassroomTemplate template)
        {
            if (id != template.Id) return NotFound();

            if (User.IsInRole("Partner"))
            {
                var userId = _userManager.GetUserId(User);
                var existing = await _context.ClassroomTemplates.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                if (existing == null || existing.PartnerId != userId)
                    return Forbid();
            }

            if (!ModelState.IsValid) return View(template);

            _context.Update(template);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: ClassroomTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var template = await _context.ClassroomTemplates
                .Include(t => t.Partner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (template == null) return NotFound();

            if (User.IsInRole("Partner"))
            {
                var userId = _userManager.GetUserId(User);
                if (template.PartnerId != userId)
                    return Forbid();
            }

            return View(template);
        }
        // POST: ClassroomTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var template = await _context.ClassroomTemplates.FindAsync(id);
            if (template == null) return NotFound();

            if (User.IsInRole("Partner"))
            {
                var userId = _userManager.GetUserId(User);
                if (template.PartnerId != userId)
                    return Forbid();
            }

            _context.ClassroomTemplates.Remove(template);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> MyTemplates()
        {
            var userId = _userManager.GetUserId(User);
            var templates = await _context.ClassroomTemplates
                .Where(t => t.PartnerId == userId)
                .ToListAsync();

            return View("Index", templates); // Reuse Index view
        }
    }
}

using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Helpers;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = "Partner,Admin")]
    public class ClassroomTemplatesController : Controller
    {
        private readonly IClassroomTemplateService _templateService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ClassroomTemplatesController(
            IClassroomTemplateService templateService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env)
        {
            _templateService = templateService;
            _userManager = userManager;
            _env = env;
        }

        // GET: /ClassroomTemplates/
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Partner"))
            {
                var partnerId = _userManager.GetUserId(User);
                var templates = await _templateService.GetByPartnerIdAsync(partnerId);
                return View("Index", templates.Select(t => t.ToVM()));
            }

            var allTemplates = await _templateService.GetAllAsync();
            return View("Index", allTemplates.Select(t => t.ToVM()));
        }

        // GET: /ClassroomTemplates/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null) return NotFound();

            if (User.IsInRole("Partner") && template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            return View("Detail", template.ToVM());
        }

        // GET: /ClassroomTemplates/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: /ClassroomTemplates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassroomTemplateVM vm)
        {
            if (!ModelState.IsValid)
                return View("Create", vm);

            string partnerId = _userManager.GetUserId(User);
            string? imagePath = null;

            if (vm.ImageFile != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads/templates");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await vm.ImageFile.CopyToAsync(stream);
                imagePath = "/uploads/templates/" + fileName;
            }

            var entity = vm.ToEntity(partnerId, imagePath);
            await _templateService.AddAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        // GET: /ClassroomTemplates/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null) return NotFound();

            if (User.IsInRole("Partner") && template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            return View("Edit", template.ToVM());
        }

        // POST: /ClassroomTemplates/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClassroomTemplateVM vm)
        {
            if (!ModelState.IsValid)
                return View("Edit", vm);

            var existing = await _templateService.GetByIdAsync(vm.Id);
            if (existing == null) return NotFound();

            if (User.IsInRole("Partner") && existing.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            string? imagePath = existing.ImageURL;

            if (vm.ImageFile != null)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads/templates");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await vm.ImageFile.CopyToAsync(stream);
                imagePath = "/uploads/templates/" + fileName;
            }

            // Update fields
            existing.Title = vm.Title;
            existing.Description = vm.Description;
            existing.ImageURL = imagePath;
            existing.LanguageLevel = vm.LanguageLevel;
            existing.StartDate = vm.StartDate;
            existing.EndDate = vm.EndDate;
            existing.SessionTime = vm.SessionTime;
            existing.Status = vm.Status;
            existing.Price = vm.Price;

            await _templateService.UpdateAsync(existing);
            return RedirectToAction(nameof(Index));
        }

        // GET: /ClassroomTemplates/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null) return NotFound();

            if (User.IsInRole("Partner") && template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            return View("Delete", template.ToVM());
        }

        // POST: /ClassroomTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null) return NotFound();

            if (User.IsInRole("Partner") && template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            await _templateService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

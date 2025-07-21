using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Helpers;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = UserRoles.Partner + "," + UserRoles.Admin)]
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
        public async Task<IActionResult> MyTemplate()
        {
            var partnerId = _userManager.GetUserId(User);
            var templates = User.IsInRole(UserRoles.Partner)
                ? await _templateService.GetByPartnerIdAsync(partnerId)
                : await _templateService.GetAllAsync();

            var vmList = templates.Select(t => ClassroomTemplateVM.FromEntity(t));
            return View(vmList);
        }

        // GET: /ClassroomTemplates/Details/5
        public async Task<IActionResult> Detail(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null) return NotFound();
            if (User.IsInRole(UserRoles.Partner) && template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            return View(template.ToVM());
        }

        // GET: /ClassroomTemplates/Create
        public IActionResult Create() => View();

        // POST: /ClassroomTemplates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassroomTemplateVM vm)
        {
            // Danh sách định dạng cho phép
            var allowedExtensions = new[] {".pdf", ".doc", ".docx", ".xls", ".xlsx" };

            // Kiểm tra file ảnh
            if (vm.ImageFile != null)
            {
                var ext = Path.GetExtension(vm.ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "Chỉ được upload file ảnh hoặc tài liệu Word/Excel/PDF.");
                }
            }

            // Kiểm tra file tài liệu
            if (vm.DocumentFile != null)
            {
                var ext = Path.GetExtension(vm.DocumentFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("DocumentFile", "Chỉ được upload file ảnh hoặc tài liệu Word/Excel/PDF.");
                }
            }

            // Nếu có lỗi -> trả lại view
            if (!ModelState.IsValid)
                return View(vm);

            var partnerId = _userManager.GetUserId(User);

            var imageUrl = vm.ImageFile != null
                ? await SaveFileAsync(vm.ImageFile, "templates/images")
                : vm.ImageURL;

            var documentUrl = vm.DocumentFile != null
                ? await SaveFileAsync(vm.DocumentFile, "templates/docs")
                : null;

            var entity = vm.ToEntity(partnerId, imageUrl, documentUrl);
            await _templateService.AddAsync(entity);
            return RedirectToAction("Index", "Loading", new { returnUrl = "/ClassroomTemplates/MyTemplate" });
        }

        // GET: /ClassroomTemplates/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null) return NotFound();
            if (User.IsInRole(UserRoles.Partner) && template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            return View(template.ToVM());
        }

        // POST: /ClassroomTemplates/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassroomTemplateVM vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var existing = await _templateService.GetByIdAsync(id);
            if (existing == null) return NotFound();
            if (User.IsInRole(UserRoles.Partner) && existing.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            if (vm.ImageFile != null)
                existing.ImageURL = await SaveFileAsync(vm.ImageFile, "templates/images");

            if (vm.DocumentFile != null)
                existing.DocumentURL = await SaveFileAsync(vm.DocumentFile, "templates/docs");

            existing.Title = vm.Title;
            existing.Description = vm.Description;
            existing.LanguageLevel = vm.LanguageLevel;

            await _templateService.UpdateAsync(existing);
            return RedirectToAction("Index", "Loading", new { returnUrl = "/ClassroomTemplates/MyTemplate" }); //fixed
            //return RedirectToAction(nameof(Index));
        }

        // GET: /ClassroomTemplates/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null) return NotFound();
            if (User.IsInRole(UserRoles.Partner) && template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            return View(template.ToVM());
        }

        // POST: /ClassroomTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var template = await _templateService.GetByIdAsync(id);
            if (template == null) return NotFound();
            if (User.IsInRole(UserRoles.Partner) && template.PartnerId != _userManager.GetUserId(User))
                return Forbid();

            await _templateService.DeleteAsync(id);
            return RedirectToAction("Index", "Loading", new { returnUrl = "/ClassroomTemplates/MyTemplate" }); //fixed
            //return RedirectToAction(nameof(Index));
        }

        // ✅ UTILITY: Internal helper for saving file
        private async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploads);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploads, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{folder}/{fileName}";
        }
    }
}

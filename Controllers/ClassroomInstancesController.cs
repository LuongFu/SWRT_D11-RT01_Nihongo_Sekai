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

            var vm = new ClassroomInstanceDetailVM
            {
                Instance = instance,
                Template = instance.Template,
                EnrollmentCount = instance.Enrollments.Count,
                PartnerName = instance.Template.Partner.FullName
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
    }
}

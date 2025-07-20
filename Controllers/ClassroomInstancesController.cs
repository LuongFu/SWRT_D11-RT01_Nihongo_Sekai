using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Helpers;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Controllers
{
    public class ClassroomInstancesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ClassroomInstancesController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClassroomInstancesController(AppDbContext context, UserManager<ApplicationUser> userManager, ILogger<ClassroomInstancesController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
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
            instance.MaxCapacity = vm.MaxCapacity;
            instance.GoogleMeetLink = vm.GoogleMeetLink;
            instance.ClassTime = TimeSpan.FromHours(vm.SessionDurationHours);
            instance.Status = vm.Status; 

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
                vm.LanguageLevel = template.LanguageLevel;
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
                .AsSplitQuery()
                .AsNoTracking()
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

            // Kiểm tra quyền truy cập
            if (isLearner && !instance.Enrollments.Any(e => e.LearnerId == userId && !e.HasLeft))
                return Forbid();

            if (isPartner && instance.Template.PartnerId != userId)
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

                if (isPartner && finalAssignment.Submissions != null)
                {
                    allSubmissions = finalAssignment.Submissions.ToList();
                }
            }

            // Feedback logic
            var userFeedback = isLearner
                ? await _context.ClassroomFeedbacks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.ClassroomInstanceId == id && f.LearnerId == userId)
                : null;

            var feedbacks = await _context.ClassroomFeedbacks
                .Include(f => f.Learner)
                .Where(f => f.ClassroomInstanceId == id)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            var resources = await _context.ClassroomResources
                .Where(r => r.ClassroomInstanceId == id)
                .ToListAsync();

            var vm = new ClassroomContentVM
            {
                Instance = instance,
                Template = instance.Template,
                PartnerName = instance.Template.Partner?.FullName,
                FinalAssignment = finalAssignment,
                Submission = submission,
                HasSubmitted = submission != null,
                HasReviewed = userFeedback != null,
                UserFeedback = userFeedback,   // NEW
                AllSubmissions = allSubmissions,
                Resources = resources,
                Feedbacks = feedbacks
            };

            ViewBag.CurrentUserId = _userManager.GetUserId(User);
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
            var enrollment = instance.Enrollments.FirstOrDefault(e => e.LearnerId == userId);
            if (enrollment == null)
            {
                _context.ClassroomEnrollments.Add(new ClassroomEnrollment
                {
                    LearnerId = userId,
                    InstanceId = id,
                    IsPaid = instance.Price > 0, // Trả phí = true, miễn phí = false
                    EnrolledAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id });
        }

        [Authorize(Roles = "Partner")]
        [HttpPost]
        public async Task<IActionResult> UploadResource(int classroomId, List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Json(new { success = false, message = "Không có file nào được chọn!" });

            var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx", ".png", ".jpg", ".jpeg", ".zip" };

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    return Json(new
                    {
                        success = false,
                        message = $"File \"{file.FileName}\" có định dạng không hợp lệ! " +
                                  $"Chỉ chấp nhận: {string.Join(", ", allowedExtensions)}"
                    });
                }
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/resources");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var resource = new ClassroomResource
                {
                    ClassroomInstanceId = classroomId,
                    FileName = file.FileName,
                    FilePath = "/uploads/resources/" + fileName,
                    UploadedAt = DateTime.UtcNow
                };
                _context.ClassroomResources.Add(resource);
            }

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Upload thành công!",
                newResources = _context.ClassroomResources
        .Where(r => r.ClassroomInstanceId == classroomId)
        .Select(r => new { r.Id, r.FileName, UploadedAt = r.UploadedAt.ToString("dd/MM/yyyy") })
        .ToList()
            });
        }

        [Authorize(Roles = "Partner")]
        [HttpPost]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _context.ClassroomResources.FindAsync(id);
            if (resource == null)
                return Json(new { success = false, message = "Tài liệu không tồn tại!" });

            try
            {
                // Xóa file vật lý
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, resource.FilePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                _context.ClassroomResources.Remove(resource);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = $"Đã xóa tài liệu \"{resource.FileName}\"." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi khi xóa file: " + ex.Message });
            }
        }

        [Authorize(Roles = "Learner,Partner,Admin")]
        public async Task<IActionResult> DownloadResource(int id)
        {
            var resource = await _context.ClassroomResources.FindAsync(id);
            if (resource == null) return NotFound();

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, resource.FilePath.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var mimeType = "application/octet-stream";
            return PhysicalFile(filePath, mimeType, resource.FileName);
        }

        [HttpGet]
        public async Task<IActionResult> Index(ClassroomSearchVM filter, int page = 1)
        {
            var query = _context.ClassroomInstances
                .Include(ci => ci.Template)
                .AsQueryable();

            // Keyword search
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(ci => ci.Template.Title.Contains(filter.Keyword)
                                        || ci.Template.Description.Contains(filter.Keyword));
            }

            // Language level
            if (filter.LanguageLevel.HasValue)
            {
                query = query.Where(ci => ci.Template.LanguageLevel == filter.LanguageLevel.Value);
            }

            // Price range
            if (filter.MinPrice.HasValue)
                query = query.Where(ci => ci.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(ci => ci.Price <= filter.MaxPrice.Value);

            // Start date
            if (filter.StartDate.HasValue)
                query = query.Where(ci => ci.StartDate.Date >= filter.StartDate.Value.Date);

            // Paging (nếu cần)
            int pageSize = 9;
            var totalItems = await query.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Levels = Enum.GetValues(typeof(LanguageLevel))
                .Cast<LanguageLevel>()
                .Select(e => new SelectListItem
                {
                    Text = GetDisplayName(e),
                    Value = e.ToString(),
                    Selected = (filter.LanguageLevel == e)
                }).ToList();



            var classrooms = await query
                .OrderBy(ci => ci.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ci => new ClassroomInstanceVM
                {
                    Id = ci.Id,
                    TemplateTitle = ci.Template.Title,
                    TemplateDescription = ci.Template.Description,
                    LanguageLevel = ci.Template.LanguageLevel, // <-- Lấy từ Template
                    Price = ci.Price,
                    StartDate = ci.StartDate,
                    EndDate = ci.EndDate,
                    TemplateImageURL = ci.Template.ImageURL
                })
                .ToListAsync();

            ViewBag.Filter = filter; // Để giữ giá trị filter trên UI
            return View(classrooms);
        }
        private string GetDisplayName(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DisplayAttribute)field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            return attribute?.Name ?? value.ToString();
        }

        [Authorize(Roles = UserRoles.Learner)]
        [HttpPost]
        public async Task<IActionResult> SubmitReview(int instanceId, int rating, string comment)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var instance = await _context.ClassroomInstances
                .Include(i => i.Feedbacks)
                .FirstOrDefaultAsync(i => i.Id == instanceId);

            if (instance == null)
                return NotFound();

            // Kiểm tra Learner có trong Enrollments
            var enrolled = await _context.ClassroomEnrollments
                .AnyAsync(e => e.InstanceId == instanceId && e.LearnerId == userId && !e.HasLeft);

            if (!enrolled)
                return Forbid();

            // Kiểm tra feedback đã tồn tại
            var existingFeedback = await _context.ClassroomFeedbacks
                .FirstOrDefaultAsync(f => f.ClassroomInstanceId == instanceId && f.LearnerId == userId);

            if (existingFeedback != null)
            {
                // Cập nhật feedback
                existingFeedback.Rating = rating;
                existingFeedback.Comment = comment;
                existingFeedback.UpdatedAt = DateTime.UtcNow;
                _context.Update(existingFeedback);
            }
            else
            {
                // Thêm feedback mới
                var feedback = new ClassroomFeedback
                {
                    ClassroomInstanceId = instanceId,
                    LearnerId = userId,
                    Rating = rating,
                    Comment = comment,
                    CreatedAt = DateTime.UtcNow
                };
                _context.ClassroomFeedbacks.Add(feedback);
            }

            await _context.SaveChangesAsync();
            TempData["FeedbackMessage"] = "Đã gửi feedback thành công!";

            return RedirectToAction("Content", new { id = instanceId });
        }

        // Learner: Join free class
        [Authorize(Roles = UserRoles.Learner)]
        public async Task<IActionResult> JoinFreeClass(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var instance = await _context.ClassroomInstances
                .Include(i => i.Enrollments)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instance == null) return NotFound();

            // Nếu lớp đã đầy
            if (instance.Enrollments.Count >= instance.MaxCapacity)
            {
                TempData["ErrorMessage"] = "Số lượng học viên tham gia đã đủ.";
                return RedirectToAction("Details", new { id });
            }

            // Nếu đã tham gia rồi thì vào thẳng Content
            if (instance.Enrollments.Any(e => e.LearnerId == userId))
                return RedirectToAction("Content", new { id });

            // Thêm learner vào lớp (miễn phí => IsPaid = true)
            _context.ClassroomEnrollments.Add(new ClassroomEnrollment
            {
                LearnerId = userId,
                InstanceId = id,
                IsPaid = true,
                EnrolledAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return RedirectToAction("Content", new { id });
        }

        // Gửi tin nhắn chat (Learner/Partner)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendChatMessage(int classroomId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return BadRequest(new { error = "Tin nhắn không được để trống." });

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // Kiểm tra quyền (Learner hoặc Partner của lớp này)
            bool isMember = await _context.ClassroomEnrollments
                .AnyAsync(e => e.InstanceId == classroomId && e.LearnerId == user.Id);

            bool isPartner = await _context.ClassroomInstances
                .AnyAsync(ci => ci.Id == classroomId && ci.Template.PartnerId == user.Id);

            if (!isMember && !isPartner)
                return Forbid();

            try
            {
                var chatMessage = new ClassroomChatMessage
                {
                    ClassroomInstanceId = classroomId,
                    UserId = user.Id,
                    Message = message.Trim(),
                    SentAt = DateTime.UtcNow
                };

                _context.ClassroomChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    userName = user.FullName ?? user.Email,
                    message = chatMessage.Message,
                    sentAt = chatMessage.SentAt.ToLocalTime().ToString("HH:mm dd/MM")
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[SendChatMessage] Error khi gửi tin nhắn cho Classroom {classroomId}");
                return StatusCode(500, new { error = $"Lỗi khi gửi tin nhắn: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetChatMessages(int classroomId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            bool isMember = await _context.ClassroomEnrollments
                .AnyAsync(e => e.InstanceId == classroomId && e.LearnerId == user.Id);

            bool isPartner = await _context.ClassroomInstances
                .Include(ci => ci.Template)
                .AnyAsync(ci => ci.Id == classroomId && ci.Template.PartnerId == user.Id);

            if (!isMember && !isPartner)
                return Forbid();

            try
            {
                var currentUserId = _userManager.GetUserId(User);

                var messages = await _context.ClassroomChatMessages
                    .AsNoTracking()
                    .Include(m => m.User)
                    .Where(m => m.ClassroomInstanceId == classroomId)
                    .OrderBy(m => m.SentAt)
                    .Select(m => new
                    {
                        userName = !string.IsNullOrEmpty(m.User.FullName) ? m.User.FullName : m.User.Email,
                        message = m.Message,
                        sentAt = m.SentAt.ToLocalTime().ToString("HH:mm dd/MM"),
                        isOwn = m.UserId == currentUserId  // Thêm cờ isOwn
                    })
                    .ToListAsync();

                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetChatMessages] Error khi tải tin nhắn của Classroom {classroomId}");
                return StatusCode(500, new { error = $"Lỗi khi tải tin nhắn: {ex.Message}" });
            }
        }
    }
}

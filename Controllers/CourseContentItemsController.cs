using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Controllers
{
    public class CourseContentItemsController : Controller
    {
        private readonly ICourseContentItemsService _service;
        private readonly IVideosService _videosService;
        private readonly IQuizzesService _quizzesService;
        private readonly ICoursesService _courseService;
        private readonly ICourseSectionsService _sectionService;
        private readonly AppDbContext _context;

        public CourseContentItemsController(
            ICourseContentItemsService service,
            IVideosService videosService,
            IQuizzesService quizzesService,
            ICoursesService courseService,
            ICourseSectionsService sectionService,
            AppDbContext context)
        {
            _service = service;
            _videosService = videosService;
            _quizzesService = quizzesService;
            _courseService = courseService;
            _sectionService = sectionService;
            _context = context;
        }

        // GET: /CourseContentItems/Create?courseId=2&sectionId=3
        [HttpGet]
        public async Task<IActionResult> Create(int courseId, int sectionId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            var section = await _context.CourseSections.FirstOrDefaultAsync(s => s.Id == sectionId);

            if (course == null || section == null) return NotFound();

            var quizzes = await _context.Quizzes
                .Where(q => q.CourseId == courseId)
                .Select(q => new SelectListItem { Text = q.Title, Value = q.Id.ToString() })
                .ToListAsync();

            var vm = new NewCourseContentItemVM
            {
                CourseId = courseId,
                SectionId = sectionId,
                CourseTitle = course.Name,
                SectionTitle = section.Title,
                Quizzes = quizzes
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Create(NewCourseContentItemVM vm)
        {
            if (!ModelState.IsValid)
            {
                // Rebuild lại các dropdown nếu có lỗi
                vm.Quizzes = await _context.Quizzes
                    .Where(q => q.CourseId == vm.CourseId)
                    .Select(q => new SelectListItem { Text = q.Title, Value = q.Id.ToString() })
                    .ToListAsync();

                return View(vm);
            }

            // Map từ ViewModel sang Entity
            var contentItem = new CourseContentItem
            {
                Title = vm.Title,
                SectionId = vm.SectionId,
                DisplayOrder = vm.DisplayOrder ?? 0,
                ContentType = vm.ContentType
            };

            if (vm.ContentType == ContentType.Video)
            {
                contentItem.Video = new Video
                {
                    VideoURL = vm.VideoURL,
                    VideoDescription = vm.VideoDescription
                };
            }
            //else if (vm.ContentType == ContentType.Quiz)
            //{
            //    contentItem.QuizId = vm.QuizId;
            //}
            else if (vm.ContentType == ContentType.Quiz)
            {
                if (!string.IsNullOrWhiteSpace(vm.NewQuizTitle))
                {
                    // Tạo mới quiz
                    var newQuiz = new Quiz
                    {
                        Title = vm.NewQuizTitle,
                        CourseId = vm.CourseId
                    };

                    _context.Quizzes.Add(newQuiz);
                    await _context.SaveChangesAsync();

                    contentItem.QuizId = newQuiz.Id;
                }
                else if (vm.QuizId.HasValue)
                {
                    // Gán quiz đã chọn từ dropdown
                    contentItem.QuizId = vm.QuizId.Value;
                }
                else
                {
                    ModelState.AddModelError("", "Please select or create a quiz.");
                    vm.Quizzes = await _context.Quizzes
                        .Where(q => q.CourseId == vm.CourseId)
                        .Select(q => new SelectListItem { Text = q.Title, Value = q.Id.ToString() })
                        .ToListAsync();

                    return View(vm);
                }
            }

            await _service.AddAsync(contentItem);
            return RedirectToAction("Details", "Courses", new { id = vm.CourseId });
        }



        // GET: /CourseContentItems/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            var section = await _context.CourseSections
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == item.SectionId);

            if (section == null) return NotFound();

            var vm = new NewCourseContentItemVM
            {
                Id = item.Id,
                Title = item.Title,
                ContentType = item.ContentType,
                SectionId = item.SectionId,
                CourseId = section.CourseId,
                CourseTitle = section.Course.Name,
                SectionTitle = section.Title,
                QuizId = item.QuizId,
                VideoURL = item.Video?.VideoURL,
                VideoDescription = item.Video?.VideoDescription,
                Quizzes = await _context.Quizzes
                    .Where(q => q.CourseId == section.CourseId)
                    .Select(q => new SelectListItem { Text = q.Title, Value = q.Id.ToString() })
                    .ToListAsync()
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewCourseContentItemVM vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                // Rebuild dropdowns nếu có lỗi
                vm.Quizzes = await _context.Quizzes
                    .Where(q => q.CourseId == vm.CourseId)
                    .Select(q => new SelectListItem { Text = q.Title, Value = q.Id.ToString() })
                    .ToListAsync();

                return View(vm);
            }

            // Lấy item từ DB và cập nhật
            var existingItem = await _service.GetByIdAsync(id);
            if (existingItem == null) return NotFound();

            existingItem.Title = vm.Title;
            existingItem.ContentType = vm.ContentType;
            existingItem.QuizId = (vm.ContentType == ContentType.Quiz) ? vm.QuizId : null;

            if (vm.ContentType == ContentType.Video)
            {
                if (existingItem.Video == null)
                {
                    existingItem.Video = new Video();
                }

                existingItem.Video.VideoURL = vm.VideoURL;
                existingItem.Video.VideoDescription = vm.VideoDescription;
            }

            await _service.UpdateAsync(existingItem);
            return RedirectToAction("Details", "Courses", new { id = vm.CourseId });
        }


        // GET: CourseContentItems/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.CourseContentItems
                .Include(ci => ci.Section)  // 👈 Load Section để lấy CourseId
                .FirstOrDefaultAsync(ci => ci.Id == id);

            if (item == null) return NotFound();

            return View(item);
        }

        // POST: CourseContentItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            var courseId = item.Section?.CourseId ?? 0;

            await _service.DeleteAsync(id);
            return RedirectToAction("Details", "Courses", new { id = courseId });
        }
    }
}

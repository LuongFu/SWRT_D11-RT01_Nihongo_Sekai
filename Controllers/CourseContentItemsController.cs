using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Controllers
{
    public class CourseContentItemsController : Controller
    {
        private readonly ICourseContentItemsService _service;
        private readonly IVideosService _videosService;
        private readonly IQuizzesService _quizzesService;

        public CourseContentItemsController(
            ICourseContentItemsService service,
            IVideosService videosService,
            IQuizzesService quizzesService)
        {
            _service = service;
            _videosService = videosService;
            _quizzesService = quizzesService;
        }

        // GET: /CourseContentItems/Create?sectionId=10
        public async Task<IActionResult> Create(int sectionId)
        {
            ViewBag.SectionId = sectionId;
            ViewBag.Videos = new SelectList(await _videosService.GetAllAsync(), "Id", "VideoDescription");
            ViewBag.Quizzes = new SelectList(await _quizzesService.GetAllAsync(), "Id", "Title");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseContentItem item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SectionId = item.SectionId;
                ViewBag.Videos = new SelectList(await _videosService.GetAllAsync(), "Id", "VideoDescription");
                ViewBag.Quizzes = new SelectList(await _quizzesService.GetAllAsync(), "Id", "Title");
                return View(item);
            }

            await _service.AddAsync(item);
            return RedirectToAction("Details", "Courses", new { id = item.Section.CourseId });
        }

        // GET: /CourseContentItems/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            ViewBag.Videos = new SelectList(await _videosService.GetAllAsync(), "Id", "VideoDescription");
            ViewBag.Quizzes = new SelectList(await _quizzesService.GetAllAsync(), "Id", "Title");
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CourseContentItem item)
        {
            if (id != item.Id) return BadRequest();
            if (!ModelState.IsValid) return View(item);

            await _service.UpdateAsync(item);
            return RedirectToAction("Details", "Courses", new { id = item.Section.CourseId });
        }

        // GET: /CourseContentItems/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            int courseId = item.Section.CourseId;
            await _service.DeleteAsync(id);
            return RedirectToAction("Details", "Courses", new { id = courseId });
        }
    }
}

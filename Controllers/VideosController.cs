using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class VideosController : Controller
    {
        private readonly IVideosService _service;

        public VideosController(IVideosService service)
        {
            _service = service;
        }

        // GET: Videos
        // Cho phép mọi người (AllowAnonymous) xem toàn bộ video, không phân trang
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allVideos = await _service.GetAllAsync();
            return View(allVideos); // model is IEnumerable<Video>
        }

        // GET: Videos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Videos/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoURL,VideoDescription")] Video video)
        {
            if (!ModelState.IsValid)
                return View(video);

            await _service.AddAsync(video);
            return RedirectToAction(nameof(Index));
        }

        // GET: Videos/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var videoDetails = await _service.GetByIdAsync(id);
            if (videoDetails == null)
                return View("NotFound");

            return View(videoDetails);
        }

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var videoDetails = await _service.GetByIdAsync(id);
            if (videoDetails == null)
                return View("NotFound");

            return View(videoDetails);
        }

        // POST: Videos/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VideoURL,VideoDescription")] Video video)
        {
            if (!ModelState.IsValid)
                return View(video);

            await _service.UpdateAsync(id, video);
            return RedirectToAction(nameof(Index));
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var videoDetails = await _service.GetByIdAsync(id);
            if (videoDetails == null)
                return View("NotFound");

            return View(videoDetails);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videoDetails = await _service.GetByIdAsync(id);
            if (videoDetails == null)
                return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

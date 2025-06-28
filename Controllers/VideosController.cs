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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: Videos/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("VideoURL,VideoDescription")]Video video)
        {
            if (!ModelState.IsValid)
            {
                return View(video);
            }
            await _service.AddAsync(video);
            return RedirectToAction(nameof(Index));
        }

        //Get: Videos/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var videoDetails = await _service.GetByIdAsync(id);

            if (videoDetails == null) return View("NotFound");
            return View(videoDetails);
        }

        //Get: Videos/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var videoDetails = await _service.GetByIdAsync(id);
            if (videoDetails == null) return View("NotFound");
            return View(videoDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VideoURL,VideoDescription")] Video video)
        {
            if (!ModelState.IsValid)
            {
                return View(video);
            }
            await _service.UpdateAsync(id, video);
            return RedirectToAction(nameof(Index));
        }

        //Get: Videos/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var videoDetails = await _service.GetByIdAsync(id);
            if (videoDetails == null) return View("NotFound");
            return View(videoDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videoDetails = await _service.GetByIdAsync(id);
            if (videoDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

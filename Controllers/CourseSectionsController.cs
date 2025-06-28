using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class CourseSectionsController : Controller
{
    private readonly ICourseSectionsService _service;

    public CourseSectionsController(ICourseSectionsService service)
    {
        _service = service;
    }

    // GET: CourseSections/Create?courseId=1
    public IActionResult Create(int courseId)
    {
        var viewModel = new CourseSectionVM
        {
            CourseId = courseId
        };

        return View(viewModel);
    }

    // POST: CourseSections/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseSectionVM viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var section = new CourseSection
        {
            Title = viewModel.Title,
            CourseId = viewModel.CourseId
        };

        await _service.AddAsync(section);
        return RedirectToAction("Details", "Courses", new { id = viewModel.CourseId });
    }

    // GET: CourseSections/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var section = await _service.GetByIdAsync(id);
        if (section == null) return NotFound();

        var viewModel = new CourseSectionVM
        {
            Id = section.Id,
            Title = section.Title,
            CourseId = section.CourseId
        };

        return View(viewModel);
    }

    // POST: CourseSections/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CourseSectionVM viewModel)
    {
        if (id != viewModel.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var section = new CourseSection
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            CourseId = viewModel.CourseId
        };

        await _service.UpdateAsync(section);
        return RedirectToAction("Details", "Courses", new { id = viewModel.CourseId });
    }

    // GET: CourseSections/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var section = await _service.GetByIdAsync(id);
        if (section == null) return View("NotFound");
        return View(section); // Nếu bạn KHÔNG dùng view này thì hãy xóa hẳn Action này đi.
    }

    // POST: CourseSections/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var section = await _service.GetByIdAsync(id);
        if (section == null) return View("NotFound");

        await _service.DeleteAsync(id);
        return RedirectToAction("Details", "Courses", new { id = section.CourseId });
    }


}

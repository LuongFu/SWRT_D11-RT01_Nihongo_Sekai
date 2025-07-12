using Microsoft.AspNetCore.Mvc;
using JapaneseLearningPlatform.Data;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.ViewComponents
{
    public class DailyWordPopupViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public DailyWordPopupViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allWords = await _context.DailyWords.OrderBy(w => w.Id).ToListAsync();
            if (!allWords.Any()) return View(null);

            var daysSinceStart = (DateTime.Today - new DateTime(2025, 1, 1)).Days;
            var index = daysSinceStart % allWords.Count;
            var wordOfTheDay = allWords[index];

            return View(wordOfTheDay);
        }
    }
}

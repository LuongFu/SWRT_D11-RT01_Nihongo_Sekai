using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JapaneseLearningPlatform.Controllers
{
    [Route("dictionary")]
    public class DictionaryController : Controller
    {
        private readonly HttpClient _httpClient;

        public DictionaryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? keyword, int? level)
        {
            string url = "https://jlpt-vocab-api.vercel.app/api/words?";

            if (!string.IsNullOrWhiteSpace(keyword))
                url += $"word={Uri.EscapeDataString(keyword)}&";

            if (level.HasValue)
                url += $"level={level}&";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return View(new List<DictionaryWord>());

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<JLPTResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            ViewBag.Keyword = keyword;
            ViewBag.Level = level;

            return View(result?.Words ?? new List<DictionaryWord>());
        }


    }
}

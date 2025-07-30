// File: Controllers/DictionaryController.cs
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Controllers
{
    [Route("dictionary")]
    public class DictionaryController : Controller
    {
        private readonly HttpClient _httpClient;
        private const int FetchLimit = 10000;

        public DictionaryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? keyword, int? level)
        {
            // build URL with a high limit so we get every word up front
            var url = $"https://jlpt-vocab-api.vercel.app/api/words?limit={FetchLimit}&";
            if (!string.IsNullOrWhiteSpace(keyword))
                url += $"word={Uri.EscapeDataString(keyword)}&";
            if (level.HasValue)
                url += $"level={level}&";

            var allWords = new List<DictionaryWord>();
            var resp = await _httpClient.GetAsync(url);
            if (resp.IsSuccessStatusCode)
            {
                var json = await resp.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JLPTResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                allWords = result?.Words ?? new List<DictionaryWord>();

                // in-memory multi-field filter if keyword
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    var kw = keyword.Trim();
                    var kwLower = kw.ToLowerInvariant();
                    allWords = allWords
                      .Where(w =>
                         // kanji
                         (w.Word ?? "").IndexOf(kw, StringComparison.InvariantCultureIgnoreCase) >= 0
                      // furigana
                      || (w.Furigana ?? "").IndexOf(kw, StringComparison.InvariantCultureIgnoreCase) >= 0
                      // romaji
                      || ((w.Romaji ?? "").ToLowerInvariant().Contains(kwLower))
                      // meaning
                      || ((w.Meaning ?? "").ToLowerInvariant().Contains(kwLower))
                      )
                      .ToList();
                }
            }

            ViewBag.Keyword = keyword ?? "";
            ViewBag.Level = level;
            return View("Search", allWords);
        }
    }
}

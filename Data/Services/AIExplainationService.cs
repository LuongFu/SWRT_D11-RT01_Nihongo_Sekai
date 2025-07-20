using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace JapaneseLearningPlatform.Data.Services
{
    public class AIExplanationService : IAIExplanationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AIExplanationService(HttpClient httpClient, IOptions<AIOptions> options)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.GeminiApiKey;
            _httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/");
        }

        public async Task<string> GetExplanationAsync(string question, string userAnswer, string correctAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer)) userAnswer = "[Không có đáp án]";
            if (string.IsNullOrWhiteSpace(correctAnswer)) correctAnswer = "[Không có đáp án đúng]";

            var prompt = $"Giải thích tại sao câu trả lời đúng cho câu hỏi '{question}' là '{correctAnswer}' và vì sao người dùng trả lời '{userAnswer}' là sai. Giải thích trong một đoạn văn ngắn.";

            var requestBody = new
            {
                contents = new[]
                {
            new {
                role = "user",
                parts = new[] { new { text = prompt } }
            }
        }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"models/gemini-2.0-flash:generateContent?key={_apiKey}", content);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            if (!json.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                return "[AI Error]: No AI response.";

            var parts = candidates[0].GetProperty("content").GetProperty("parts");
            if (parts.GetArrayLength() == 0)
                return "[AI Error]: No content parts.";

            return parts[0].GetProperty("text").GetString();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace JapaneseLearningPlatform.Models
{
    public class DictionaryWord
    {
        [JsonPropertyName("word")]
        public string Word { get; set; }

        [JsonPropertyName("meaning")]
        public string Meaning { get; set; }

        [JsonPropertyName("furigana")]
        public string Furigana { get; set; }

        [JsonPropertyName("romaji")]
        public string Romaji { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }
    }

}

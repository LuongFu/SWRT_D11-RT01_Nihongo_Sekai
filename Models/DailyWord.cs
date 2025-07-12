using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Models
{
    public class DailyWord
    {
        public int Id { get; set; }

        [Required]
        public string JapaneseWord { get; set; } // e.g., 勉強

        public string Romanji { get; set; } // e.g., benkyou

        public string Description { get; set; } // e.g., học tập
        public string ImageUrl { get; set; } // optional, e.g., URL to an image representing the word
    }

}

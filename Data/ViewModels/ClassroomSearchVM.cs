using JapaneseLearningPlatform.Data.Enums;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class ClassroomSearchVM
    {
        public string Keyword { get; set; } // Tìm theo tên hoặc mô tả
        public LanguageLevel? LanguageLevel { get; set; } // Enum string: N5, N4...
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? StartDate { get; set; }
    }

}

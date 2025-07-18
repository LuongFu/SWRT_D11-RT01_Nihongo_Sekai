using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.Enums
{
    public enum SpecializationType
    {
        [Display(Name = "Luyện hội thoại")]
        ConversationPractice = 0,   // Luyện hội thoại

        [Display(Name = "Tiếng Nhật thương mại")]
        BusinessJapanese = 1,       // Tiếng Nhật thương mại

        [Display(Name = "Chuẩn bị JLPT")]
        JLPTPreparation = 2,        // Chuẩn bị JLPT

        [Display(Name = "Ngữ pháp")]
        Grammar = 3,                // Ngữ pháp

        [Display(Name = "Văn hóa Nhật")]
        CulturalStudies = 4,        // Văn hóa Nhật

        [Display(Name = "Phát âm")]
        Pronunciation = 5           // Phát âm
    }
}

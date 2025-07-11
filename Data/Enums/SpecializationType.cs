using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.Enums
{
    public enum SpecializationType
    {
        [Display(Name = "Conversation Practice")]
        ConversationPractice = 0,   // Luyện hội thoại

        [Display(Name = "Business Japanese")]
        BusinessJapanese = 1,       // Tiếng Nhật thương mại

        [Display(Name = "JLPT Preparation")]
        JLPTPreparation = 2,        // Chuẩn bị JLPT

        [Display(Name = "Grammar")]
        Grammar = 3,                // Ngữ pháp

        [Display(Name = "Cultural Studies")]
        CulturalStudies = 4,        // Văn hóa Nhật

        [Display(Name = "Pronunciation")]
        Pronunciation = 5           // Phát âm
    }
}

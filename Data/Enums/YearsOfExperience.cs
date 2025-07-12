using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.Enums
{
    public enum YearsOfExperience
    {
        [Display(Name = "1-2 years")]
        OneToTwo = 0,       // 1–2 năm kinh nghiệm

        [Display(Name = "3-5 years")]
        ThreeToFive = 1,    // 3–5 năm kinh nghiệm

        [Display(Name = "6-10 years")]
        SixToTen = 2,       // 6–10 năm kinh nghiệm

        [Display(Name = "10+ years")]
        MoreThanTen = 3     // Trên 10 năm kinh nghiệm
    }
}

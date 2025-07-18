using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.Enums
{
    public enum CourseCategory
    {
        [Display(Name = "Bảng chữ cái")]
        Alphabet = 1,

        [Display(Name = "Cơ bản")]
        Basic,

        [Display(Name = "Trung cấp")]
        Intermediate,

        [Display(Name = "Nâng cao")]
        Advanced,

        [Display(Name = "Văn hóa")]
        Culture
    }
}

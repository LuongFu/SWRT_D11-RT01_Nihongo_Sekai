using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.Enums
{
    public enum LanguageLevel
    {
        [Display(Name = "N5 - Sơ cấp")]
        N5 = 1,

        [Display(Name = "N4 - Cơ bản")]
        N4 = 2,

        [Display(Name = "N3 - Trung cấp")]
        N3 = 3,

        [Display(Name = "N2 - Cao cấp")]
        N2 = 4,

        [Display(Name = "N1 - Thượng cấp")]
        N1 = 5
    }
}

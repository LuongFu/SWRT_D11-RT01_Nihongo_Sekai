using System.ComponentModel.DataAnnotations;

namespace JapaneseLearningPlatform.Data.Enums
{
    public enum ContentType
    {
        [Display(Name = "Video")]
        Video = 0,

        [Display(Name = "Quiz")]
        Quiz = 1
    }
}

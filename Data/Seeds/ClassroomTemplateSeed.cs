using JapaneseLearningPlatform.Data.Enums;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class ClassroomTemplateSeed
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public LanguageLevel LanguageLevel { get; set; }
        public string PartnerEmail { get; set; } = "";
        public string ImageURL { get; set; } = "";
    }

}

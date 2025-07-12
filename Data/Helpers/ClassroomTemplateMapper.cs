using JapaneseLearningPlatform.Models;
using JapaneseLearningPlatform.Data.ViewModels;

namespace JapaneseLearningPlatform.Helpers
{
    public static class ClassroomTemplateMapper
    {
        public static ClassroomTemplateVM ToVM(this ClassroomTemplate entity)
        {
            return new ClassroomTemplateVM
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                ImageURL = entity.ImageURL,
                DocumentURL = entity.DocumentURL, 
                LanguageLevel = entity.LanguageLevel
            };
        }

        public static ClassroomTemplate ToEntity(this ClassroomTemplateVM vm, string partnerId, string? imagePath = null, string? documentPath = null)
        {
            return new ClassroomTemplate
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                ImageURL = imagePath,
                DocumentURL = documentPath,
                LanguageLevel = vm.LanguageLevel,
                PartnerId = partnerId
            };
        }
    }
}

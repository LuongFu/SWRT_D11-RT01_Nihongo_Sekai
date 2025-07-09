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
                LanguageLevel = entity.LanguageLevel,
                SessionTime = entity.SessionTime,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Price = entity.Price,
                Status = entity.Status
            };
        }

        public static ClassroomTemplate ToEntity(this ClassroomTemplateVM vm, string partnerId, string? imagePath = null)
        {
            return new ClassroomTemplate
            {
                Title = vm.Title,
                Description = vm.Description,
                ImageURL = imagePath,
                LanguageLevel = vm.LanguageLevel,
                SessionTime = vm.SessionTime,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                Price = vm.Price,
                Status = vm.Status,
                PartnerId = partnerId
            };
        }
    }
}

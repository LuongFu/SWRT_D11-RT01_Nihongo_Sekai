using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using System.Linq;

public static class ClassroomInstanceMapper
{
    public static ClassroomInstanceVM ToVM(this ClassroomInstance entity)
    {
        if (entity == null) return null;

        var template = entity.Template;

        return new ClassroomInstanceVM
        {
            Id = entity.Id,
            TemplateId = entity.TemplateId,

            // ✅ Thông tin Template (nếu có)
            TemplateTitle = template?.Title ?? string.Empty,
            TemplateDescription = template?.Description ?? string.Empty,
            TemplateImageURL = template?.ImageURL ?? string.Empty,
            LanguageLevel = template?.LanguageLevel.ToString() ?? string.Empty,
            DocumentURL = template?.DocumentURL,

            // ✅ Thông tin phiên học
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            SessionDurationHours = (int)entity.ClassTime.TotalHours,
            Price = entity.Price,
            GoogleMeetLink = entity.GoogleMeetLink,
            Status = entity.Status,

            // ✅ Trạng thái và thống kê
            IsPaid = entity.IsPaid,
            EnrollmentCount = entity.Enrollments?.Count(e => !e.HasLeft) ?? 0
        };
    }
}

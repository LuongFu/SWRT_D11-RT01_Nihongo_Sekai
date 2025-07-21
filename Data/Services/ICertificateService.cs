using Microsoft.AspNetCore.Mvc;
namespace JapaneseLearningPlatform.Data.Services
{

    public interface ICertificateService
    {
        Task<CourseCertificate> GenerateAsync(string userId, int courseId, IUrlHelper urlHelper);
        Task<List<CourseCertificate>> GetByUserAsync(string userId);
    }
}
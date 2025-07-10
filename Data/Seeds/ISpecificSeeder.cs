using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public interface ISpecificSeeder
    {
        Task SeedAsync(AppDbContext context, IServiceProvider services);
    }
}

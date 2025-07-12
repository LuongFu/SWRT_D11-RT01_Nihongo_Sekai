using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class VideoSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            if (!context.Videos.Any())
            {
                context.Videos.AddRange(StaticSeedData.Videos); // takes video data from StaticSeedData
                await context.SaveChangesAsync();
            }
        }
    }
}

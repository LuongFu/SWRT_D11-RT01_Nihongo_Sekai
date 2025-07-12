using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class DailyWordSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            // If already seeded, skip
            if (context.DailyWords.Any()) return;

            // Get initial daily words from static data
            context.DailyWords.AddRange(StaticSeedData.DailyWords);
            await context.SaveChangesAsync();
        }
    }
}

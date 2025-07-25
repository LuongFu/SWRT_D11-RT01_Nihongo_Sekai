﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public static class SeedManager
    {
        public static async Task SeedAllAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var services = scope.ServiceProvider;

            context.Database.EnsureCreated();

            var seeders = new ISpecificSeeder[]
            {
            new VideoSeeder(),
            new CourseSeeder(),
            new QuizSeeder(),
            new UserSeeder(),
            new ClassroomTemplateSeeder(),
            new ClassroomInstanceSeeder(),
            new ClassroomTestEnrollmentSeeder(),
            new ClassroomAssignmentSeeder(),
            new DailyWordSeeder(),
            new ReportSeeder(),
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(context, services);
            }
        }
    }

}

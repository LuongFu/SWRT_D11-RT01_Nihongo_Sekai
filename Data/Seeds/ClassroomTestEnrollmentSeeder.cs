using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class ClassroomTestEnrollmentSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            // Retrieve UserManager to lookup learner by email
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            const string learnerEmail = "noobhoang@gmail.com";
            var learner = await userManager.FindByEmailAsync(learnerEmail);

            // Find an in-progress classroom instance
            var instance = await context.ClassroomInstances
                .FirstOrDefaultAsync(c => c.Status == ClassroomStatus.InProgress);

            if (learner == null || instance == null)
                return;

            // Check if already enrolled
            bool already = await context.ClassroomEnrollments
                .AnyAsync(e => e.InstanceId == instance.Id && e.LearnerId == learner.Id);

            if (!already)
            {
                context.ClassroomEnrollments.Add(new ClassroomEnrollment
                {
                    InstanceId = instance.Id,
                    LearnerId = learner.Id,
                    EnrolledAt = DateTime.UtcNow,
                    IsPaid = false,
                    HasLeft = false
                });
                await context.SaveChangesAsync();
            }
        }
    }
}

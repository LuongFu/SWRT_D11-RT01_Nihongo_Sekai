using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class ClassroomAssignmentSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            // Ensure database created
            context.Database.EnsureCreated();

            // Fetch any classroom instance and learner
            var instance = await context.ClassroomInstances.FirstOrDefaultAsync();
            var learner = await context.Users.FirstOrDefaultAsync(u => u.Role == UserRoles.Learner);

            if (instance == null || learner == null)
                return;

            // Seed final assignment if missing
            bool hasAssignment = await context.FinalAssignments
                .AnyAsync(a => a.ClassroomInstanceId == instance.Id);

            if (!hasAssignment)
            {
                var assignment = new FinalAssignment
                {
                    ClassroomInstanceId = instance.Id,
                    Instructions = "Please submit a short essay about your classroom experience.",
                    DueDate = DateTime.UtcNow.AddDays(7)
                };
                context.FinalAssignments.Add(assignment);
                await context.SaveChangesAsync();

                // Add a submission
                context.AssignmentSubmissions.Add(new AssignmentSubmission
                {
                    FinalAssignmentId = assignment.Id,
                    LearnerId = learner.Id,
                    SubmittedAt = DateTime.UtcNow,
                    AnswerText = "This is my essay submission.",
                    Score = 8.5,
                    Feedback = "Good job overall."
                });

                // Add an evaluation
                context.ClassroomEvaluations.Add(new ClassroomEvaluation
                {
                    InstanceId = instance.Id,
                    LearnerId = learner.Id,
                    Score = 9.0,
                    Comment = "Great learning experience!",
                    EvaluatedAt = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
            }
        }
    }
}

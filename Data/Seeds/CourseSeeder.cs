using JapaneseLearningPlatform.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class CourseSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider provider)
        {
            if (context.Courses.Any()) return;

            foreach (var cs in StaticSeedData.Courses)
            {
                var course = new Course
                {
                    Name = cs.Name,
                    Description = cs.Description,
                    Price = cs.Price,
                    ImageURL = cs.ImageURL,
                    DiscountPercent = cs.Discount,
                    CourseCategory = cs.Category,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(30)
                };
                context.Courses.Add(course);
                await context.SaveChangesAsync();

                // sections
                var sections = cs.Sections
                    .Select(t => new CourseSection { Title = t, CourseId = course.Id })
                    .ToList();
                context.CourseSections.AddRange(sections);
                await context.SaveChangesAsync();

                // quiz
                var quiz = new Quiz
                {
                    Title = cs.QuizTitle,
                    CourseId = course.Id
                };
                context.Quizzes.Add(quiz);
                await context.SaveChangesAsync();

                // questions
                foreach (var q in cs.Questions)
                    q.QuizId = quiz.Id;
                context.QuizQuestions.AddRange(cs.Questions);
                await context.SaveChangesAsync();

                // content items
                var items = cs.Contents.Select(c => new CourseContentItem
                {
                    Title = c.Title,
                    SectionId = (c.SectionIndex >= 0 && c.SectionIndex < sections.Count)
    ? sections[c.SectionIndex].Id
    : throw new InvalidOperationException($"Invalid SectionIndex {c.SectionIndex} for content '{c.Title}' in course '{course.Name}'"),
                    ContentType = c.Type,
                    VideoId = c.VideoId,
                    QuizId = c.QuizIndex.HasValue ? quiz.Id : null
                }).ToList();
                context.CourseContentItems.AddRange(items);
                await context.SaveChangesAsync();
            }
        }
    }
}

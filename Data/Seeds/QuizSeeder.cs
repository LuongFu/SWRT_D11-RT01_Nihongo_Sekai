using JapaneseLearningPlatform.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class QuizSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            if (context.Quizzes.Any()) return;

            foreach (var (courseSeed, course) in StaticSeedData.Courses.Zip(context.Courses.Include(c => c.Sections), (s, c) => (s, c)))
            {
                if (courseSeed.Questions is null)
                    continue;

                // 1. Create new quiz
                var quiz = new Quiz
                {
                    Title = courseSeed.QuizTitle,
                    Questions = new List<QuizQuestion>()
                };

                // 2. Add quiz questions
                foreach (var q in courseSeed.Questions)
                {
                    var question = new QuizQuestion
                    {
                        QuestionText = q.QuestionText,
                        QuestionType = q.QuestionType,
                        Options = q.Options.Select(o => new QuizOption
                        {
                            OptionText = o.OptionText,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                    };
                    quiz.Questions.Add(question);
                }

                await context.Quizzes.AddAsync(quiz);
                await context.SaveChangesAsync();

                // 3. Find ContentItem where Type = Quiz
                foreach (var contentSeed in courseSeed.Contents.Where(x => x.Type == ContentType.Quiz))
                {
                    var section = course.Sections.ElementAtOrDefault(contentSeed.SectionIndex);
                    if (section == null) continue;

                    var contentItem = new CourseContentItem
                    {
                        Title = contentSeed.Title,
                        ContentType = ContentType.Quiz,
                        SectionId = section.Id,
                        QuizId = quiz.Id
                    };
                    await context.CourseContentItems.AddAsync(contentItem);
                }

                await context.SaveChangesAsync();
            }
        }
    }

}

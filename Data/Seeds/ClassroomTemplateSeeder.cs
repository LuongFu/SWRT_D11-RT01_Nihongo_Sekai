// Data/Seeds/ClassroomTemplateSeeder.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class ClassroomTemplateSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            if (context.ClassroomTemplates.Any()) return;

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var partner = await userManager.FindByEmailAsync("giakhoiquang@gmail.com");
            if (partner == null) return;

            var templates = new List<ClassroomTemplate>
            {
                new ClassroomTemplate
                {
                    Title = "Beginner Japanese Conversation",
                    Description = "Focus on daily life dialogues for beginners.",
                    LanguageLevel = LanguageLevel.N5,
                    PartnerId = partner.Id,
                    ImageURL = "https://eclectic-homeschool.com/wp-content/uploads/2014/09/beginningjapanese.jpg"
                },
                new ClassroomTemplate
                {
                    Title = "Intermediate Listening Practice",
                    Description = "Listen and discuss JLPT N4-level audios.",
                    LanguageLevel = LanguageLevel.N4,
                    PartnerId = partner.Id,
                    ImageURL = "https://static.vecteezy.com/system/resources/previews/009/385/472/original/school-desk-clipart-design-illustration-free-png.png"
                }
            };

            context.ClassroomTemplates.AddRange(templates);
            await context.SaveChangesAsync();
        }
    }
}

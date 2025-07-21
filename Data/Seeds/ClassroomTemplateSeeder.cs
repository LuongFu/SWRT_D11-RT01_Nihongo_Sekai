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
                    Title = "Beginner Japanese Conversation 1",
                    Description = "Focus on daily life dialogues for beginners.",
                    LanguageLevel = LanguageLevel.N5,
                    PartnerId = partner.Id,
                    ImageURL = "https://dungmori.b-cdn.net/assets/img/new_home/06-2024/jlpt-new-n54-banner.png"
                },
                new ClassroomTemplate
                {
                    Title = "Intermediate Listening Practice 2",
                    Description = "Listen and discuss JLPT N4-level audios.",
                    LanguageLevel = LanguageLevel.N4,
                    PartnerId = partner.Id,
                    ImageURL = "https://static.vecteezy.com/system/resources/previews/009/385/472/original/school-desk-clipart-design-illustration-free-png.png"
                },
                new ClassroomTemplate
                {
                    Title = "Intermediate Listening Practice 3",
                    Description = "Listen and discuss JLPT N4-level audios.",
                    LanguageLevel = LanguageLevel.N4,
                    PartnerId = partner.Id,
                    ImageURL = "https://i-vnexpress.vnecdn.net/2020/01/02/hoc-sinh-nhat-4649-1577932394.jpg"
                }
                ,
                new ClassroomTemplate
                {
                    Title = "Intermediate Listening Practice 4",
                    Description = "Listen and discuss JLPT N4-level audios.",
                    LanguageLevel = LanguageLevel.N4,
                    PartnerId = partner.Id,
                    ImageURL = "https://lophoctiengnhat.edu.vn/images/2017/04/19/hoc-tieng-nhat.jpg"
                }
                ,
                new ClassroomTemplate
                {
                    Title = "Intermediate Listening Practice 5",
                    Description = "Listen and discuss JLPT N4-level audios.",
                    LanguageLevel = LanguageLevel.N4,
                    PartnerId = partner.Id,
                    ImageURL = "https://kilala.vn/data/article/lop-hoc-fb.jpg"
                }
                ,
                new ClassroomTemplate
                {
                    Title = "Intermediate Listening Practice 6",
                    Description = "Listen and discuss JLPT N4-level audios.",
                    LanguageLevel = LanguageLevel.N4,
                    PartnerId = partner.Id,
                    ImageURL = "https://static.edupia.vn/dungchung/dungchung/core_cms/resources/uploads/common/images/2023/11/07/edupia-pro.jpg"
                }
            };

            context.ClassroomTemplates.AddRange(templates);
            await context.SaveChangesAsync();
        }
    }
}

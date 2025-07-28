// Data/Seeds/ClassroomInstanceSeeder.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class ClassroomInstanceSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            if (context.ClassroomInstances.Any()) return;

            var template = await context.ClassroomTemplates
                                        .FirstOrDefaultAsync();
            if (template == null) return;

            var instances = new List<ClassroomInstance>
            {
                new ClassroomInstance
                {
                    TemplateId = template.Id,
                    StartDate = DateTime.Today.AddDays(3),
                    EndDate = DateTime.Today.AddDays(33),
                    ClassTime = new TimeSpan(19, 0, 0),
                    MaxCapacity = 10,
                    Price = 130000,
                    IsPaid = true,
                    GoogleMeetLink = "uj9d-xzho-vasm",
                    Status = ClassroomStatus.Published
                },
                new ClassroomInstance
                {
                    TemplateId = template.Id,
                    StartDate = DateTime.Today.AddDays(-10),
                    EndDate = DateTime.Today.AddDays(20),
                    ClassTime = new TimeSpan(20, 0, 0),
                    MaxCapacity = 8,
                    Price = 0,
                    GoogleMeetLink = "uj9d-xzho-vasm",
                    Status = ClassroomStatus.InProgress
                },
                new ClassroomInstance
                {
                    TemplateId = template.Id,
                    StartDate = DateTime.Today.AddDays(-10),
                    EndDate = DateTime.Today.AddDays(20),
                    ClassTime = new TimeSpan(20, 0, 0),
                    MaxCapacity = 8,
                    Price = 240000,
                    GoogleMeetLink = "uj9d-xzho-vasm",
                    Status = ClassroomStatus.InProgress
                },
                new ClassroomInstance
                {
                    TemplateId = template.Id,
                    StartDate = DateTime.Today.AddDays(-10),
                    EndDate = DateTime.Today.AddDays(20),
                    ClassTime = new TimeSpan(20, 0, 0),
                    MaxCapacity = 8,
                    Price = 300000,
                    GoogleMeetLink = "uj9d-xzho-vasm",
                    Status = ClassroomStatus.InProgress
                },
                new ClassroomInstance
                {
                    TemplateId = template.Id,
                    StartDate = DateTime.Today.AddDays(-10),
                    EndDate = DateTime.Today.AddDays(20),
                    ClassTime = new TimeSpan(20, 0, 0),
                    MaxCapacity = 7,
                    Price = 500000,
                    GoogleMeetLink = "uj9d-xzho-vasm",
                    Status = ClassroomStatus.InProgress
                },
                new ClassroomInstance
                {
                    TemplateId = template.Id,
                    StartDate = DateTime.Today.AddDays(-10),
                    EndDate = DateTime.Today.AddDays(20),
                    ClassTime = new TimeSpan(20, 0, 0),
                    MaxCapacity = 8,
                    Price = 140000,
                    GoogleMeetLink = "uj9d-xzho-vasm",
                    Status = ClassroomStatus.InProgress
                }
            };

            context.ClassroomInstances.AddRange(instances);
            foreach (var instance in instances)
            {
                if (instance.Price > 0)
                {
                    instance.IsPaid = true;
                }
            }
            await context.SaveChangesAsync();
        }
    }
}

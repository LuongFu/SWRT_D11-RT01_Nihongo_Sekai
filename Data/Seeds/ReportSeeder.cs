// File: Data/Seeds/ReportSeeder.cs
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class ReportSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider provider)
        {
            var vms = StaticSeedData.Reports;
            var roles = new[] { "Learner", "Partner", "Guest" };
            var now = DateTime.UtcNow;

            var toInsert = new List<Report>();

            for (int i = 0; i < vms.Count; i++)
            {
                var vm = vms[i];

                // Nếu email đã có trong DB → bỏ qua
                if (await context.Reports.AnyAsync(r => r.Email == vm.Email))
                    continue;

                // Validate view‑model
                var errors = vm.Validate(new ValidationContext(vm)).ToList();
                if (errors.Any())
                    continue;

                toInsert.Add(new Report
                {
                    FullName = vm.FullName,
                    Email = vm.Email,
                    Subject = vm.Subject,
                    OrderNumber = vm.Subject == ReportSubject.Billing ? vm.OrderNumber : null,
                    Message = vm.Message,
                    Role = roles[i % roles.Length],
                    SubmittedAt = now
                                    .AddDays(-(i % 10))
                                    .AddHours(i % 24)
                                    .AddMinutes((i * 7) % 60),
                    IsResolved = false
                });
            }

            if (toInsert.Count > 0)
            {
                await context.Reports.AddRangeAsync(toInsert);
                await context.SaveChangesAsync();
            }
        }
    }

}

// Data/Seeds/UserSeeder.cs
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JapaneseLearningPlatform.Data.Seeds
{
    public class UserSeeder : ISpecificSeeder
    {
        public async Task SeedAsync(AppDbContext context, IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Seed roles
            foreach (var role in StaticSeedData.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Seed users
            foreach (var u in StaticSeedData.Users)
            {
                // nếu email đã tồn tại thì bỏ qua
                var existing = await userManager.FindByEmailAsync(u.Email);
                if (existing != null) continue;

                var user = new ApplicationUser
                {
                    FullName = u.FullName,
                    UserName = u.Email,
                    Email = u.Email,
                    EmailConfirmed = true,
                    Role = u.Role,
                    IsApproved = true,
                    IsBanned = u.IsBanned
                };

                var creation = await userManager.CreateAsync(user, u.Password);
                if (!creation.Succeeded) continue;

                // gán role
                await userManager.AddToRoleAsync(user, u.Role);

                // lưu thêm field Role & IsBanned
                await userManager.UpdateAsync(user);
            }
        }
    }
}

using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                
                //Videos
                if (!context.Videos.Any())
                {
                    context.Videos.AddRange(new List<Video>()
                    {
                        new Video()
                        {
                            VideoURL = "https://www.youtube.com/watch?v=rGrBHiuPlT0&ab_channel=JapanSocietyNYC",
                            VideoDescription = "This is the Description of the first video",


                        },
                        new Video()
                        {
                            VideoURL = "https://www.youtube.com/watch?v=bOUqVC4XkOY&ab_channel=JapanSocietyNYC",
                            VideoDescription = "This is the Description of the second video"

                        },
                        new Video()
                        {
                            VideoURL = "https://www.youtube.com/watch?v=JnoZE51WZg4&t=1s&ab_channel=JapanSocietyNYC",
                            VideoDescription = "This is the Description of the second video"
                           
                        },
                        new Video()
                        {   VideoURL = "https://www.youtube.com/watch?v=KUIWRsVZZZA&t=5s&ab_channel=JapanSocietyNYC",
                            VideoDescription = "This is the Description of the second video"
                            
                        },
                        new Video()
                        {
                            VideoURL = "https://www.youtube.com/watch?v=SzG9STImtk0&ab_channel=HarupakaJapanese",
                            VideoDescription = "This is the Description of the second video"

                        }
                    });
                    context.SaveChanges();
                }
                //Courses
                if (!context.Courses.Any())
                {
                    context.Courses.AddRange(new List<Course>()
                    {
                        new Course()
                        {
                            Name = "Nihongo Lesson ep 1",
                            Description = "Basic Japanese Lesson 1",
                            Price = 7.80,
                            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731338/japanese_lesson_1_epmipj.jpg",
                            StartDate = DateTime.Now.AddDays(-10),
                            EndDate = DateTime.Now.AddDays(10),
                            CourseCategory = CourseCategory.Alphabet
                        },
                        new Course()
                        {
                            Name = "Nihongo Lesson ep 2",
                            Description = "Basic Japanese Lesson 2",
                            Price = 16.80,
                            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731340/japanese_lesson_2_ktfwkn.jpg",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(3),
                            CourseCategory = CourseCategory.Basic
                        },
                        new Course()
                        {
                            Name = "Nihongo Lesson ep 3",
                            Description = "Basic Japanese Lesson 3",
                            Price = 13.30,
                            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731343/japanese_lesson_3_w221xl.jpg",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(7),
                            CourseCategory = CourseCategory.Intermediate
                        },
                        new Course()
                        {
                            Name = "Nihongo Lesson ep 4",
                            Description = "Basic Japanese Lesson 4",
                            Price = 10.50,
                            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731369/japanese_lesson_4_odxddf.jpg",
                            StartDate = DateTime.Now.AddDays(-10),
                            EndDate = DateTime.Now.AddDays(-5),
                            CourseCategory = CourseCategory.Advanced
                        },
                        new Course()
                        {
                            Name = "Nihongo Lesson ep 5",
                            Description = "Basic Japanese Lesson 5",
                            Price = 11.50,
                            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731372/japanese_lesson_5_ruumi6.jpg",
                            StartDate = DateTime.Now.AddDays(-10),
                            EndDate = DateTime.Now.AddDays(-2),
                            CourseCategory = CourseCategory.Advanced
                        },
                        new Course()
                        {
                            Name = "Japanese with Harupaka ",
                            Description = "Basic Japanese Lesson - (ただいま)",
                            Price = 23.50,
                            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749737125/japanese_lesson_6_adt5j9.jpg",
                            StartDate = DateTime.Now.AddDays(3),
                            EndDate = DateTime.Now.AddDays(20),
                            CourseCategory = CourseCategory.Culture
                        }
                    });
                    context.SaveChanges();
                }
                //Videos & Courses
                if (!context.Videos_Courses.Any())
                {
                    context.Videos_Courses.AddRange(new List<Video_Course>()
                    {
                        new Video_Course()
                        {
                            VideoId = 1,
                            CourseId = 1
                        },
                        new Video_Course()
                        {
                            VideoId = 3,
                            CourseId = 1
                        },

                         new Video_Course()
                        {
                            VideoId = 1,
                            CourseId = 2
                        },
                         new Video_Course()
                        {
                            VideoId = 4,
                            CourseId = 2
                        },

                        new Video_Course()
                        {
                            VideoId = 1,
                            CourseId = 3
                        },
                        new Video_Course()
                        {
                            VideoId = 2,
                            CourseId = 3
                        },
                        new Video_Course()
                        {
                            VideoId = 5,
                            CourseId = 3
                        },


                        new Video_Course()
                        {
                            VideoId = 2,
                            CourseId = 4
                        },
                        new Video_Course()
                        {
                            VideoId = 3,
                            CourseId = 4
                        },
                        new Video_Course()
                        {
                            VideoId = 4,
                            CourseId = 4
                        },


                        new Video_Course()
                        {
                            VideoId = 2,
                            CourseId = 5
                        },
                        new Video_Course()
                        {
                            VideoId = 3,
                            CourseId = 5
                        },
                        new Video_Course()
                        {
                            VideoId = 4,
                            CourseId = 5
                        },
                        new Video_Course()
                        {
                            VideoId = 5,
                            CourseId = 5
                        },


                        new Video_Course()
                        {
                            VideoId = 3,
                            CourseId = 6
                        },
                        new Video_Course()
                        {
                            VideoId = 4,
                            CourseId = 6
                        },
                        new Video_Course()
                        {
                            VideoId = 5,
                            CourseId = 6
                        },
                    });
                    context.SaveChanges();
                }
            }

        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Partner))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Partner));
                if (!await roleManager.RoleExistsAsync(UserRoles.Learner))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Learner));
                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                // admin account if not exists
                string adminUserEmail = "lmp14589@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if(adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = adminUserEmail,
                        UserName = adminUserEmail,
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Phu123123@");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
                // partner account if not exists
                string partnerUserEmail = "giakhoiquang@gmail.com";

                var partner = await userManager.FindByEmailAsync(partnerUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Gia Khoi Partner",
                        UserName = "giakhoiquang@gmail.com",
                        Email = partnerUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Khoi2005.");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Partner);
                }
                // learner account if not exists
                string learnerUserEmail = "noobhoang@gmail.com";

                var appUser = await userManager.FindByEmailAsync(learnerUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Hoang Nguyen",
                        UserName = learnerUserEmail,
                        Email = learnerUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Hoang2005.");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.Learner);
                }
            }
        }
    }
}

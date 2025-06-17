using NihongoSekaiWebApplication_D11_RT01.Data.Static;
using NihongoSekaiWebApplication_D11_RT01.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NihongoSekaiWebApplication_D11_RT01.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //Cinema
                if (!context.Cinemas.Any())
                {
                    context.Cinemas.AddRange(new List<Cinema>()
                    {
                        new Cinema()
                        {
                            Name = "Cinema 1",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-1.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                        new Cinema()
                        {
                            Name = "Cinema 2",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-2.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                        new Cinema()
                        {
                            Name = "Cinema 3",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-3.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                        new Cinema()
                        {
                            Name = "Cinema 4",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-4.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                        new Cinema()
                        {
                            Name = "Cinema 5",
                            Logo = "http://dotnethow.net/images/cinemas/cinema-5.jpeg",
                            Description = "This is the description of the first cinema"
                        },
                    });
                    context.SaveChanges();
                }
                //Actors
                if (!context.Actors.Any())
                {
                    context.Actors.AddRange(new List<Actor>()
                    {
                        new Actor()
                        {
                            FullName = "Actor 1",
                            Bio = "This is the Bio of the first actor",
                            ProfilePictureURL = "http://dotnethow.net/images/actors/actor-1.jpeg"

                        },
                        new Actor()
                        {
                            FullName = "Actor 2",
                            Bio = "This is the Bio of the second actor",
                            ProfilePictureURL = "http://dotnethow.net/images/actors/actor-2.jpeg"
                        },
                        new Actor()
                        {
                            FullName = "Actor 3",
                            Bio = "This is the Bio of the second actor",
                            ProfilePictureURL = "http://dotnethow.net/images/actors/actor-3.jpeg"
                        },
                        new Actor()
                        {
                            FullName = "Actor 4",
                            Bio = "This is the Bio of the second actor",
                            ProfilePictureURL = "http://dotnethow.net/images/actors/actor-4.jpeg"
                        },
                        new Actor()
                        {
                            FullName = "Actor 5",
                            Bio = "This is the Bio of the second actor",
                            ProfilePictureURL = "http://dotnethow.net/images/actors/actor-5.jpeg"
                        }
                    });
                    context.SaveChanges();
                }
                //Producers
                if (!context.Producers.Any())
                {
                    context.Producers.AddRange(new List<Producer>()
                    {
                        new Producer()
                        {
                            FullName = "Producer 1",
                            Bio = "This is the Bio of the first actor",
                            ProfilePictureURL = "http://dotnethow.net/images/producers/producer-1.jpeg"

                        },
                        new Producer()
                        {
                            FullName = "Producer 2",
                            Bio = "This is the Bio of the second actor",
                            ProfilePictureURL = "http://dotnethow.net/images/producers/producer-2.jpeg"
                        },
                        new Producer()
                        {
                            FullName = "Producer 3",
                            Bio = "This is the Bio of the second actor",
                            ProfilePictureURL = "http://dotnethow.net/images/producers/producer-3.jpeg"
                        },
                        new Producer()
                        {
                            FullName = "Producer 4",
                            Bio = "This is the Bio of the second actor",
                            ProfilePictureURL = "http://dotnethow.net/images/producers/producer-4.jpeg"
                        },
                        new Producer()
                        {
                            FullName = "Producer 5",
                            Bio = "This is the Bio of the second actor",
                            ProfilePictureURL = "http://dotnethow.net/images/producers/producer-5.jpeg"
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
                            CinemaId = 3,
                            ProducerId = 3,
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
                            CinemaId = 1,
                            ProducerId = 1,
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
                            CinemaId = 4,
                            ProducerId = 4,
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
                            CinemaId = 1,
                            ProducerId = 2,
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
                            CinemaId = 1,
                            ProducerId = 3,
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
                            CinemaId = 1,
                            ProducerId = 5,
                            CourseCategory = CourseCategory.Culture
                        }
                    });
                    context.SaveChanges();
                }
                //Actors & Courses
                if (!context.Actors_Courses.Any())
                {
                    context.Actors_Courses.AddRange(new List<Actor_Course>()
                    {
                        new Actor_Course()
                        {
                            ActorId = 1,
                            CourseId = 1
                        },
                        new Actor_Course()
                        {
                            ActorId = 3,
                            CourseId = 1
                        },

                         new Actor_Course()
                        {
                            ActorId = 1,
                            CourseId = 2
                        },
                         new Actor_Course()
                        {
                            ActorId = 4,
                            CourseId = 2
                        },

                        new Actor_Course()
                        {
                            ActorId = 1,
                            CourseId = 3
                        },
                        new Actor_Course()
                        {
                            ActorId = 2,
                            CourseId = 3
                        },
                        new Actor_Course()
                        {
                            ActorId = 5,
                            CourseId = 3
                        },


                        new Actor_Course()
                        {
                            ActorId = 2,
                            CourseId = 4
                        },
                        new Actor_Course()
                        {
                            ActorId = 3,
                            CourseId = 4
                        },
                        new Actor_Course()
                        {
                            ActorId = 4,
                            CourseId = 4
                        },


                        new Actor_Course()
                        {
                            ActorId = 2,
                            CourseId = 5
                        },
                        new Actor_Course()
                        {
                            ActorId = 3,
                            CourseId = 5
                        },
                        new Actor_Course()
                        {
                            ActorId = 4,
                            CourseId = 5
                        },
                        new Actor_Course()
                        {
                            ActorId = 5,
                            CourseId = 5
                        },


                        new Actor_Course()
                        {
                            ActorId = 3,
                            CourseId = 6
                        },
                        new Actor_Course()
                        {
                            ActorId = 4,
                            CourseId = 6
                        },
                        new Actor_Course()
                        {
                            ActorId = 5,
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

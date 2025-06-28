using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.Static;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

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
                if (!context.Courses.Any(c => c.Id <= 6))
                {
                    var courses = new List<Course>
    {
        new Course
        {
            Name = "Nihongo Lesson ep 1",
            Description = "Basic Japanese Lesson 1",
            Price = 7.80,
            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731338/japanese_lesson_1_epmipj.jpg",
            StartDate = DateTime.Now.AddDays(-10),
            EndDate = DateTime.Now.AddDays(10),
            CourseCategory = CourseCategory.Alphabet
        },
        new Course
        {
            Name = "Nihongo Lesson ep 2",
            Description = "Basic Japanese Lesson 2",
            Price = 16.80,
            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731340/japanese_lesson_2_ktfwkn.jpg",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3),
            CourseCategory = CourseCategory.Basic
        },
        new Course
        {
            Name = "Nihongo Lesson ep 3",
            Description = "Basic Japanese Lesson 3",
            Price = 13.30,
            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731343/japanese_lesson_3_w221xl.jpg",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7),
            CourseCategory = CourseCategory.Intermediate
        },
        new Course
        {
            Name = "Nihongo Lesson ep 4",
            Description = "Basic Japanese Lesson 4",
            Price = 10.50,
            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731369/japanese_lesson_4_odxddf.jpg",
            StartDate = DateTime.Now.AddDays(-10),
            EndDate = DateTime.Now.AddDays(-5),
            CourseCategory = CourseCategory.Advanced
        },
        new Course
        {
            Name = "Nihongo Lesson ep 5",
            Description = "Basic Japanese Lesson 5",
            Price = 11.50,
            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749731372/japanese_lesson_5_ruumi6.jpg",
            StartDate = DateTime.Now.AddDays(-10),
            EndDate = DateTime.Now.AddDays(-2),
            CourseCategory = CourseCategory.Advanced
        },
        new Course
        {
            Name = "Japanese with Harupaka",
            Description = "Basic Japanese Lesson - (ただいま)",
            Price = 23.50,
            ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749737125/japanese_lesson_6_adt5j9.jpg",
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddDays(20),
            CourseCategory = CourseCategory.Culture
        }
    };

                    context.Courses.AddRange(courses);
                    context.SaveChanges();

                    int courseIndex = 1;
                    var videos = context.Videos.Take(6).ToList();
                    foreach (var course in courses)
                    {
                        var section1 = new CourseSection { Title = $"Section A of {course.Name}", CourseId = course.Id };
                        var section2 = new CourseSection { Title = $"Quiz Section of {course.Name}", CourseId = course.Id };
                        context.CourseSections.AddRange(section1, section2);
                        context.SaveChanges();

                        var quiz = new Quiz
                        {
                            Title = $"Quiz of {course.Name}",
                            CourseId = course.Id
                        };
                        context.Quizzes.Add(quiz);
                        context.SaveChanges();

                        var questions = new List<QuizQuestion>
        {
            new QuizQuestion
            {
                QuestionText = $"Question 1 of {course.Name}",
                QuizId = quiz.Id,
                Options = new List<QuizOption>
                {
                    new QuizOption { OptionText = "Option 1", IsCorrect = true },
                    new QuizOption { OptionText = "Option 2", IsCorrect = false }
                }
            },
            new QuizQuestion
            {
                QuestionText = $"Question 2 of {course.Name}",
                QuizId = quiz.Id,
                Options = new List<QuizOption>
                {
                    new QuizOption { OptionText = "Option A", IsCorrect = false },
                    new QuizOption { OptionText = "Option B", IsCorrect = true }
                }
            }
        };
                        context.QuizQuestions.AddRange(questions);
                        context.SaveChanges();

                        context.CourseContentItems.AddRange(new List<CourseContentItem>
        {
            new CourseContentItem
            {
                Title = $"Intro Video of {course.Name}",
                SectionId = section1.Id,
                ContentType = ContentType.Video,
                VideoId = videos[courseIndex - 1].Id
            },
            new CourseContentItem
            {
                Title = $"Quiz: {quiz.Title}",
                SectionId = section2.Id,
                ContentType = ContentType.Quiz,
                QuizId = quiz.Id
            }
        });
                        context.SaveChanges();

                        courseIndex++;
                    }
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
                // New Course (Course 7)
                if (!context.Courses.Any(c => c.Name == "Nihongo Lesson ep 7"))
                    {
                    var newCourse = new Course
                    {
                        Name = "Nihongo Lesson ep 7",
                        Description = "Advanced Grammar and Expressions",
                        Price = 19.99,
                        ImageURL = "https://res.cloudinary.com/dfso7lfxa/image/upload/v1749737126/japanese_lesson_7_demo.jpg",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(30),
                        CourseCategory = CourseCategory.Advanced
                    };
                    context.Courses.Add(newCourse);
                    context.SaveChanges(); // Save to get Course.Id

                    // Create 3 sections
                    var section1 = new CourseSection { Title = "Advanced Greetings", CourseId = newCourse.Id };
                    var section2 = new CourseSection { Title = "Honorific Quiz", CourseId = newCourse.Id };
                    var section3 = new CourseSection { Title = "Situational Dialogues", CourseId = newCourse.Id };
                    context.CourseSections.AddRange(section1, section2, section3);
                    context.SaveChanges(); // Save to get Section IDs

                    // Create quiz
                    var newQuiz = new Quiz
                    {
                        Title = "Honorific Expressions Quiz",
                        CourseId = newCourse.Id
                    };
                    context.Quizzes.Add(newQuiz);
                    context.SaveChanges(); // Save to get Quiz.Id

                    // Create 6 questions, each with 4 options (1 correct)
                    var quizQuestions = new List<QuizQuestion>();
                    for (int i = 1; i <= 6; i++)
                    {
                        var options = new List<QuizOption>();
                        int correctIndex = new Random().Next(0, 4);

                        for (int j = 0; j < 4; j++)
                        {
                            options.Add(new QuizOption
                            {
                                OptionText = $"Option {j + 1} for Q{i}",
                                IsCorrect = (j == correctIndex)
                            });
                        }

                        quizQuestions.Add(new QuizQuestion
                        {
                            QuestionText = $"What is the correct usage of honorific in sentence {i}?",
                            QuizId = newQuiz.Id,
                            Options = options
                        });
                    }
                    context.QuizQuestions.AddRange(quizQuestions);
                    context.SaveChanges();

                    // Add content items for sections
                    var contentItem1 = new CourseContentItem
                    {
                        Title = "Watch: Advanced Greetings Video",
                        SectionId = section1.Id,
                        ContentType = ContentType.Video,
                        VideoId = 2 // Assumes VideoId 2 exists
                    };
                    var contentItem2 = new CourseContentItem
                    {
                        Title = "Take: Honorific Quiz",
                        SectionId = section2.Id,
                        ContentType = ContentType.Quiz,
                        QuizId = newQuiz.Id
                    };

                    context.CourseContentItems.AddRange(contentItem1, contentItem2);
                    context.SaveChanges();
                }
                
                

            }

        }

        public static async Task SeedClassroomTemplatesAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!context.ClassroomTemplates.Any())
            {
                var partner = await userManager.FindByEmailAsync("giakhoiquang@gmail.com");

                if (partner != null)
                {
                    context.ClassroomTemplates.AddRange(new List<ClassroomTemplate>
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
            });

                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task SeedClassroomInstancesAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.ClassroomInstances.Any())
            {
                var template = await context.ClassroomTemplates.FirstOrDefaultAsync();

                if (template != null)
                {
                    context.ClassroomInstances.AddRange(new List<ClassroomInstance>
            {
                new ClassroomInstance
                {
                    TemplateId = template.Id,
                    StartDate = DateTime.Today.AddDays(3),
                    EndDate = DateTime.Today.AddDays(33),
                    ClassTime = new TimeSpan(19, 0, 0),
                    MaxCapacity = 10,
                    Price = 120000,
                    IsPaid = true,
                    GoogleMeetLink = "https://meet.google.com/test-class1",
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
                    IsPaid = false,
                    GoogleMeetLink = "https://meet.google.com/free-class",
                    Status = ClassroomStatus.InProgress
                }
            });

                    await context.SaveChangesAsync();
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
                    newAdminUser.Role = UserRoles.Admin;
                    await userManager.UpdateAsync(newAdminUser);
                }
                // partner account if not exists
                string partnerUserEmail = "giakhoiquang@gmail.com";

                var partner = await userManager.FindByEmailAsync(partnerUserEmail);
                if (partner == null)
                {
                    var newPartnerUser = new ApplicationUser()
                    {
                        FullName = "Gia Khoi Partner",
                        UserName = partnerUserEmail,
                        Email = partnerUserEmail,
                        EmailConfirmed = true,
                        Role = UserRoles.Partner // add this line
                    };
                    await userManager.CreateAsync(newPartnerUser, "Khoi2005.");
                    await userManager.AddToRoleAsync(newPartnerUser, UserRoles.Partner);
                    await userManager.UpdateAsync(newPartnerUser);
                }


                // Additional partner accounts
                var extraPartners = new[]
                {
    new { Email = "partner1@nihongo.com", FullName = "Partner One" },
    new { Email = "partner2@nihongo.com", FullName = "Partner Two" },
};

                foreach (var p in extraPartners)
                {
                    var found = await userManager.FindByEmailAsync(p.Email);
                    if (found == null)
                    {
                        var user = new ApplicationUser
                        {
                            FullName = p.FullName,
                            UserName = p.Email,
                            Email = p.Email,
                            EmailConfirmed = true,
                            Role = UserRoles.Partner
                        };
                        await userManager.CreateAsync(user, "Test123@"); // ✅ default password
                        await userManager.AddToRoleAsync(user, UserRoles.Partner);
                        await userManager.UpdateAsync(user);
                    }
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

                string bannedEmail = "banneduser@gmail.com";

                var bannedUser = await userManager.FindByEmailAsync(bannedEmail);
                if (bannedUser == null)
                {
                    var newBannedUser = new ApplicationUser()
                    {
                        FullName = "Banned User",
                        UserName = bannedEmail,
                        Email = bannedEmail,
                        EmailConfirmed = true,
                        IsBanned = true // ✅ set banned flag
                    };

                    await userManager.CreateAsync(newBannedUser, "Test123!@#");
                    await userManager.AddToRoleAsync(newBannedUser, UserRoles.Learner);
                }
            }
        }
    }
}

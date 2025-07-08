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

                // Word of the day
                if (!context.DailyWords.Any())
                {
                    context.DailyWords.AddRange(new List<DailyWord>
    {
        new DailyWord { JapaneseWord = "勉強", Romanji = "benkyou", Description = "Học tập, học hành (Study)", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQZtHBysJ4mTeiU03nAn9XJgr4_doYLAoudFA&s"},
        new DailyWord { JapaneseWord = "友達", Romanji = "tomodachi", Description = "Bạn bè (Friend)", ImageUrl = "https://media.istockphoto.com/id/541995888/photo/japanese-friends-group-selfie.jpg?s=612x612&w=0&k=20&c=easi1Getsu3m04cyY2vuGtznh1fTeiMJQ0AHauY7C70="},
        new DailyWord { JapaneseWord = "仕事", Romanji = "shigoto", Description = "Công việc (Career)", ImageUrl = "https://unlockjapan.jp/wp-content/uploads/2024/11/job-fair.png"},
        new DailyWord { JapaneseWord = "先生", Romanji = "sensei", Description = "Giáo viên (Teacher)", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRhqIn41d5l8hQq57u8gav9dbcQFuRAQcxtlg&s"},
        new DailyWord { JapaneseWord = "時間", Romanji = "jikan", Description = "Thời gian (Time)", ImageUrl = "https://m.media-amazon.com/images/I/51wRuQHNHJL.jpg"},
        new DailyWord { JapaneseWord = "家族", Romanji = "kazoku", Description = "Gia đình (Family)", ImageUrl = "https://kated.com/wp-content/uploads/2020/03/JPN59a-Be-A-Guest-Of-A-Japanese-Family.jpg"},
        new DailyWord { JapaneseWord = "食べ物", Romanji = "tabemono", Description = "Thức ăn, món ăn (Food)", ImageUrl = "https://rimage.gnst.jp/livejapan.com/public/article/detail/a/00/02/a0002670/img/basic/a0002670_main.jpg"},
        new DailyWord { JapaneseWord = "飲み物", Romanji = "nomimono", Description = "Đồ uống (Drink)", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ0-ReVo2VLbWu9tOAwKREJ4HNdhcWoz2sX5g&s"},
        new DailyWord { JapaneseWord = "学校", Romanji = "gakkou", Description = "Trường học (School)", ImageUrl = "https://web-japan.org/kidsweb/explore/calendar/assets/img/april/schoolyear01.jpg"},
        new DailyWord { JapaneseWord = "天気", Romanji = "tenki", Description = "Thời tiết (Weather)", ImageUrl = "https://rimage.gnst.jp/livejapan.com/public/article/detail/a/00/00/a0000213/img/basic/a0000213_main.jpg"},
        new DailyWord { JapaneseWord = "旅行", Romanji = "ryokou", Description = "Du lịch (Travel)", ImageUrl = "https://img.freepik.com/free-photo/woman-traveler-with-backpack-fushimi-inari-taisha-shrine-kyoto-japan_335224-88.jpg?semt=ais_hybrid&w=740"},
        new DailyWord { JapaneseWord = "音楽", Romanji = "ongaku", Description = "Âm nhạc (Music)", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/71/Japanese_music_icon.png/1200px-Japanese_music_icon.png"},
        new DailyWord { JapaneseWord = "本", Romanji = "hon", Description = "Sách (Book)", ImageUrl = "https://jtalkonline.com/wp-content/uploads/2020/11/Where-to-Get-Japanese-Novels-Outside-of-Japan-light-novels-scaled.jpg"},
        new DailyWord { JapaneseWord = "映画", Romanji = "eiga", Description = "Phim ảnh (Movie)", ImageUrl = "https://preview.redd.it/whats-your-favorite-film-from-japan-v0-elnpvoybgtad1.jpeg?width=1080&crop=smart&auto=webp&s=aa8a6dfc2aa7a118e02f59fa96db7396bd4e8f8e"},
        new DailyWord { JapaneseWord = "買い物", Romanji = "kaimono", Description = "Mua sắm (Shopping)", ImageUrl = "https://retailnext.net/_next/image?url=https%3A%2F%2Fimages.ctfassets.net%2Fuskqevaodrls%2F5JeL2yfgYL9POZSsLwdsnD%2Fee2260e2d93c439e30e80b8e0082cc94%2Fshutterstock_2294474927__2_.jpg&w=3840&q=75"},
        new DailyWord { JapaneseWord = "電話", Romanji = "denwa", Description = "Điện thoại (Mobile Phone)", ImageUrl = "https://www.japan-guide.com/g20/2223_02.jpg"},
        new DailyWord { JapaneseWord = "料理", Romanji = "ryouri", Description = "Nấu ăn, món ăn (Cook)", ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSzWjhP5PayWx3eRbHLf6YJxgh7cpXCT3_ozQ&s"},
        new DailyWord { JapaneseWord = "誕生日", Romanji = "tanjoubi", Description = "Sinh nhật (Birthday)", ImageUrl = "https://cdn.shopify.com/s/files/1/1083/2612/files/shutterstock_1240909954_480x480.jpg?v=1663330108"},
        new DailyWord { JapaneseWord = "動物", Romanji = "doubutsu", Description = "Động vật (Animal)", ImageUrl = "https://mlyhahhwafqm.i.optimole.com/cb:5ksX~4cf46/w:640/h:423/q:75/f:best/ig:avif/https://interacnetwork.com/the-content/cream/wp-content/uploads/2021/11/image8.jpg"},
        new DailyWord { JapaneseWord = "遊び", Romanji = "asobi", Description = "Chơi đùa, giải trí (Play, Entertainment)", ImageUrl = "https://d3c8ah58dul3sf.cloudfront.net/wp-content/uploads/2022/04/B5447956-8F73-4813-9A73-5BA0BF2DF186.jpeg"},
    });

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

        public static async Task SeedClassroomTestEnrollmentsAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var learner = await userManager.FindByEmailAsync("noobhoang@gmail.com");

            var instance = await context.ClassroomInstances
                .FirstOrDefaultAsync(c => c.Status == ClassroomStatus.InProgress); // chọn lớp free hoặc đang hoạt động

            if (learner != null && instance != null)
            {
                var alreadyEnrolled = await context.ClassroomEnrollments
                    .AnyAsync(e => e.InstanceId == instance.Id && e.LearnerId == learner.Id);

                if (!alreadyEnrolled)
                {
                    context.ClassroomEnrollments.Add(new ClassroomEnrollment
                    {
                        InstanceId = instance.Id,
                        LearnerId = learner.Id,
                        EnrolledAt = DateTime.Now,
                        IsPaid = false,
                        HasLeft = false
                    });

                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task SeedClassroomAssessmentsAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                // Get an existing instance and learner
                var instance = context.ClassroomInstances.FirstOrDefault();
                var learner = context.Users.FirstOrDefault(u => u.Role == UserRoles.Learner);

                if (instance == null || learner == null)
                    return;

                // Create FinalAssessment if not exists
                if (!context.FinalAssessments.Any(a => a.ClassroomInstanceId == instance.Id))
                {
                    var assessment = new FinalAssessment
                    {
                        ClassroomInstanceId = instance.Id,
                        Instructions = "Please submit a short essay about your classroom experience.",
                        DueDate = DateTime.UtcNow.AddDays(7)
                    };

                    context.FinalAssessments.Add(assessment);
                    await context.SaveChangesAsync();

                    // Create AssessmentSubmission
                    var submission = new AssessmentSubmission
                    {
                        FinalAssessmentId = assessment.Id,
                        LearnerId = learner.Id,
                        SubmittedAt = DateTime.UtcNow,
                        AnswerText = "This is my essay submission.",
                        Score = 8.5,
                        Feedback = "Good job overall."
                    };

                    context.AssessmentSubmissions.Add(submission);

                    // Create ClassroomEvaluation
                    var evaluation = new ClassroomEvaluation
                    {
                        InstanceId = instance.Id,
                        LearnerId = learner.Id,
                        Score = 9.0,
                        Comment = "Great learning experience!",
                        EvaluatedAt = DateTime.UtcNow
                    };

                    context.ClassroomEvaluations.Add(evaluation);

                    await context.SaveChangesAsync();
                }
            }
        }

    }
}

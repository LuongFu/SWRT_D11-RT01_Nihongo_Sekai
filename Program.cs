﻿using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Cart;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Configuration access
            var configuration = builder.Configuration;

            // DbContext configuration
            var connectionString = Environment.GetEnvironmentVariable("DB_CONN_STRING")
    ?? configuration.GetConnectionString("DefaultConnectionString");

            if (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("DefaultConnection")))
            {
                throw new InvalidOperationException("No connection string configured via environment variable or appsettings.json.");
            }

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));

            // Services configuration
            builder.Services.AddScoped<ICoursesService, CoursesService>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddScoped<IVideosService, VideosService>();
            builder.Services.AddScoped<ICourseSectionsService, CourseSectionsService>();
            builder.Services.AddScoped<ICourseContentItemsService, CourseContentItemsService>();
            builder.Services.AddScoped<IQuizzesService, QuizzesService>();
            builder.Services.AddScoped<IQuizQuestionsService, QuizQuestionsService>();

            //Classrooms:
            builder.Services.AddScoped<IClassroomTemplateService, ClassroomTemplateService>();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));

            // Email sender configuration
            builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

            // Authentication and authorization
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
                        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; //  quay lại login page tự viết
                    options.AccessDeniedPath = "/Account/AccessDenied";
                })
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
            });

            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Seed database
            AppDbInitializer.Seed(app);
            await AppDbInitializer.SeedUsersAndRolesAsync(app);
            await AppDbInitializer.SeedClassroomTemplatesAsync(app);
            await AppDbInitializer.SeedClassroomInstancesAsync(app);
            await AppDbInitializer.SeedClassroomTestEnrollmentsAsync(app);
            await AppDbInitializer.SeedClassroomAssessmentsAsync(app);

            app.Run();
        }
    }
}



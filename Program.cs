﻿using Google.Api;
using JapaneseLearningPlatform.Controllers;
using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Cart;
using JapaneseLearningPlatform.Data.Seeds;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Hubs;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using System.Net;

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

            // Services configuration
            builder.Services.AddScoped<ICoursesService, CoursesService>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddScoped<IVideosService, VideosService>();
            builder.Services.AddScoped<ICourseSectionsService, CourseSectionsService>();
            builder.Services.AddScoped<ICourseContentItemsService, CourseContentItemsService>();
            builder.Services.AddScoped<IQuizzesService, QuizzesService>();
            builder.Services.AddScoped<IQuizQuestionsService, QuizQuestionsService>();

            // Dịch vụ Course/Ratings/Certificates
            builder.Services.AddScoped<ICourseRatingService, CourseRatingService>();
            builder.Services.AddScoped<ICertificateService, CertificateService>();

            builder.Services.AddScoped<ICourseRatingService, CourseRatingService>();


            //Classrooms:
            builder.Services.AddScoped<IClassroomTemplateService, ClassroomTemplateService>();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));

            // Email sender configuration
            builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

            // VNPay
            builder.Services.AddSingleton<VNPayService>();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

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

                // ném nếu thiếu config, tránh gán null
                options.ClientId = googleAuthNSection["ClientId"]
                                      ?? throw new InvalidOperationException("Missing Authentication:Google:ClientId");
                options.ClientSecret = googleAuthNSection["ClientSecret"]
                                      ?? throw new InvalidOperationException("Missing Authentication:Google:ClientSecret");

                // Thêm đoạn sau để xử lý khi người dùng bấm Cancel hoặc lỗi xảy ra
                options.Events.OnRemoteFailure = context =>
                {
                    context.Response.Redirect("/Account/LoginFailed?error=access_denied");
                    context.HandleResponse(); // chặn xử lý mặc định (throw exception)
                    return Task.CompletedTask;
                };

                //  Force Google to always show account chooser
                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
                    return Task.CompletedTask;
                };
            });
            // add api connect
            builder.Services.AddHttpClient<DictionaryController>();
            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddControllersWithViews();

            builder.Services.AddHostedService<RejectedCleanupService>();
            // Bind AIOptions
            builder.Services.Configure<AIOptions>(builder.Configuration.GetSection("AI"));
            builder.Services
            .AddHttpClient<IAIExplanationService, AIExplanationService>()
            .AddPolicyHandler(GetRetryPolicy());

            static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError() // 5xx, 408, network failures
                    .WaitAndRetryAsync(
                        retryCount: 3,
                        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        onRetry: (outcome, timespan, attempt, context) =>
                        {
                            Console.WriteLine($"Retry {attempt} after {timespan.TotalSeconds}s due to {outcome.Result?.StatusCode}");
                        });
            }

            ///////////////////////////////////////////
            builder.Services.AddSignalR();
            ///////////////////////////////////////////

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

            ///////////////////////////////////////////
            //Chat mesage hub
            app.MapHub<ClassroomChatHub>("/classroomChatHub");
            ///////////////////////////////////////////

            // Seed database
            await SeedManager.SeedAllAsync(app); // Seed all data using the SeedManager
            app.Run();
        }
    }
}



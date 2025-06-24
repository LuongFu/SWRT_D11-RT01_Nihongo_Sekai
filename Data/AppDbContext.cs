using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // QuizResultDetail → QuizResult
            modelBuilder.Entity<QuizResultDetail>()
                .HasOne(d => d.QuizResult)
                .WithMany(r => r.Details)
                .HasForeignKey(d => d.QuizResultId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh cascade vòng

            // QuizResultDetail → QuizQuestion
            modelBuilder.Entity<QuizResultDetail>()
                .HasOne(d => d.Question)
                .WithMany()
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh cascade vòng

            // QuizResultDetail → QuizOption
            modelBuilder.Entity<QuizResultDetail>()
                .HasOne(d => d.SelectedOption)
                .WithMany()
                .HasForeignKey(d => d.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh cascade vòng

            // CourseSection → Course
            modelBuilder.Entity<CourseSection>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.Sections)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // CourseContentItem → CourseSection
            modelBuilder.Entity<CourseContentItem>()
                .HasOne(ci => ci.Section)
                .WithMany(s => s.ContentItems)
                .HasForeignKey(ci => ci.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // CourseContentItem → Video (nullable)
            modelBuilder.Entity<CourseContentItem>()
                .HasOne(ci => ci.Video)
                .WithMany()
                .HasForeignKey(ci => ci.VideoId)
                .OnDelete(DeleteBehavior.Restrict);

            // CourseContentItem → Quiz (nullable)
            modelBuilder.Entity<CourseContentItem>()
                .HasOne(ci => ci.Quiz)
                .WithMany()
                .HasForeignKey(ci => ci.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            // QuizQuestion → Quiz
            modelBuilder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(qq => qq.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            // QuizOption → QuizQuestion
            modelBuilder.Entity<QuizOption>()
                .HasOne(qo => qo.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(qo => qo.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Video_Course>().HasKey(am => new
            {
                am.VideoId,
                am.CourseId
            });

            modelBuilder.Entity<Video_Course>().HasOne(m => m.Course).WithMany(am => am.Videos_Courses).HasForeignKey(m => m.CourseId);
            modelBuilder.Entity<Video_Course>().HasOne(m => m.Video).WithMany(am => am.Videos_Courses).HasForeignKey(m => m.VideoId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Video> Videos { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Video_Course> Videos_Courses { get; set; }
        public DbSet<CourseSection> CourseSections { get; set; }
        public DbSet<CourseContentItem> CourseContentItems { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizOption> QuizOptions { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<QuizResultDetail> QuizResultDetails { get; set; }
        //Orders related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}

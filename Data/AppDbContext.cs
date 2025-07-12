using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using JapaneseLearningPlatform.Models.Partner;
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

            //for classroom instance
            modelBuilder.Entity<ClassroomInstance>().Property(c => c.Status).HasConversion<int>();
            modelBuilder.Entity<ClassroomInstance>()
    .Property(ci => ci.Price)
    .HasPrecision(10, 2);
            modelBuilder.Entity<ClassroomEnrollment>()
    .HasIndex(e => new { e.InstanceId, e.LearnerId })
    .IsUnique();

            //for language level
            modelBuilder.Entity<ClassroomTemplate>()
    .Property(c => c.LanguageLevel)
    .HasConversion<int>(); // ✅ Enum -> int


            // ClassroomEnrollment → ClassroomInstance
            modelBuilder.Entity<ClassroomEnrollment>()
                .HasOne(e => e.Instance)
                .WithMany(i => i.Enrollments)
                .HasForeignKey(e => e.InstanceId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassroomEnrollment → ApplicationUser
            modelBuilder.Entity<ClassroomEnrollment>()
                .HasOne(e => e.Learner)
                .WithMany()
                .HasForeignKey(e => e.LearnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // FinalAssessment → ClassroomInstance
            modelBuilder.Entity<FinalAssessment>()
                .HasOne(fa => fa.Instance)
                .WithMany(ci => ci.Assessments)
                .HasForeignKey(fa => fa.ClassroomInstanceId)
                .OnDelete(DeleteBehavior.Restrict);

            // AssessmentSubmission → FinalAssessment
            modelBuilder.Entity<AssessmentSubmission>()
                .HasOne(s => s.Assessment)
                .WithMany(a => a.Submissions)
                .HasForeignKey(s => s.FinalAssessmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // AssessmentSubmission → ApplicationUser
            modelBuilder.Entity<AssessmentSubmission>()
                .HasOne(s => s.Learner)
                .WithMany()
                .HasForeignKey(s => s.LearnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassroomEvaluation → ClassroomInstance
            modelBuilder.Entity<ClassroomEvaluation>()
                .HasOne(e => e.Instance)
                .WithMany()
                .HasForeignKey(e => e.InstanceId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassroomEvaluation → ApplicationUser
            modelBuilder.Entity<ClassroomEvaluation>()
                .HasOne(e => e.Learner)
                .WithMany()
                .HasForeignKey(e => e.LearnerId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Video_Course>().HasOne(m => m.Course).WithMany(am => am.Videos_Courses).HasForeignKey(m => m.CourseId);
            modelBuilder.Entity<Video_Course>().HasOne(m => m.Video).WithMany(am => am.Videos_Courses).HasForeignKey(m => m.VideoId);

            // 1) Mối quan hệ 1 PartnerProfile có nhiều PartnerDocument,
            //    nhưng khi xóa profile thì không tự động xóa document (Restrict)
            modelBuilder.Entity<PartnerDocument>()
            .HasOne(d => d.Profile)
            .WithMany(p => p.Documents)
            .HasForeignKey(d => d.PartnerProfileId)
            .OnDelete(DeleteBehavior.Cascade);


            // 2) Mối quan hệ 1-1 PartnerProfile ↔ ApplicationUser,
            //    nhưng khi xóa user thì không tự động xóa profile (Restrict)
            modelBuilder.Entity<PartnerProfile>()
            .HasOne(p => p.User)
            .WithOne(u => u.PartnerProfile)
            .HasForeignKey<PartnerProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            // ---- MỚI: cấu hình enum Status và DecisionAt ----
            modelBuilder.Entity<PartnerProfile>()
            .Property(p => p.Status)
            .HasConversion<int>()
            .HasDefaultValue(PartnerStatus.Pending);
            
            modelBuilder.Entity<PartnerProfile>()
            .Property(p => p.DecisionAt)
            .IsRequired(false);

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

        //CLASSROOMS:
        public DbSet<ClassroomTemplate> ClassroomTemplates { get; set; }
        public DbSet<ClassroomInstance> ClassroomInstances { get; set; }
        public DbSet<ClassroomEnrollment> ClassroomEnrollments { get; set; }
        public DbSet<FinalAssessment> FinalAssessments { get; set; }
        public DbSet<AssessmentSubmission> AssessmentSubmissions { get; set; }
        public DbSet<ClassroomEvaluation> ClassroomEvaluations { get; set; }

        public DbSet<DailyWord> DailyWords { get; set; }
        //public DbSet<PartnerDocument> PartnerCertificates { get; set; }
        public DbSet<PartnerProfile> PartnerProfiles { get; set; }
        public DbSet<PartnerSpecialization> PartnerSpecializations { get; set; }
        public DbSet<PartnerDocument> PartnerDocuments { get; set; }


    }
}

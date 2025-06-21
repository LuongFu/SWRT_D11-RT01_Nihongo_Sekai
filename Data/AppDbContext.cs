using JapaneseLearningPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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


        //Orders related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}

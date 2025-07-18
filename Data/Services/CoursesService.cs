﻿using JapaneseLearningPlatform.Data.Base;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace JapaneseLearningPlatform.Data.Services
{
    public class CoursesService : EntityBaseRepository<Course>, ICoursesService
    {
        private readonly AppDbContext _context;

        public CoursesService(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<CourseWithPurchaseVM>> GetAllCoursesWithPurchaseInfoAsync(string userId, string shoppingCartId)
        {
            var courses = await _context.Courses.ToListAsync();

            var purchasedCourseIds = new List<int>();
            var cartCourseIds = new List<int>();

            if (!string.IsNullOrEmpty(userId))
            {
                purchasedCourseIds = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .SelectMany(o => o.OrderItems.Select(oi => oi.CourseId))
                    .Distinct()
                    .ToListAsync();
            }

            if (!string.IsNullOrEmpty(shoppingCartId))
            {
                cartCourseIds = await _context.ShoppingCartItems
                    .Where(i => i.ShoppingCartId == shoppingCartId)
                    .Select(i => i.Course.Id)
                    .ToListAsync();
            }

            var result = courses.Select(course => new CourseWithPurchaseVM
            {
                Course = course,
                IsPurchased = purchasedCourseIds.Contains(course.Id),
                IsInCart = cartCourseIds.Contains(course.Id)
            }).ToList();

            return result;
        }

        // Thêm khóa học mới
        public async Task AddNewCourseAsync(NewCourseVM data)
        {
            if (data.DiscountPercent == 0)
            {
                data.DiscountPercent = null;
            }
            var newCourse = new Course()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageURL = data.ImageURL,
                DiscountPercent = data.DiscountPercent,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                CourseCategory = data.CourseCategory
            };
            await _context.Courses.AddAsync(newCourse);
            await _context.SaveChangesAsync();

            // Gắn video vào khóa học (many-to-many)
            foreach (var videoId in data.VideoIds)
            {
                var newVideoCourse = new Video_Course()
                {
                    CourseId = newCourse.Id,
                    VideoId = videoId
                };
                await _context.Videos_Courses.AddAsync(newVideoCourse);
            }
            await _context.SaveChangesAsync();
        }

        // Lấy chi tiết 1 khóa học bao gồm video
        public async Task<Course> GetCourseByIdAsync(int id)
        {
            var courseDetails = await _context.Courses
                .Include(vc => vc.Videos_Courses).ThenInclude(a => a.Video)
                .FirstOrDefaultAsync(n => n.Id == id);

            return courseDetails;
        }

        // Lấy danh sách video để hiển thị trong dropdown
        public async Task<NewCourseDropdownsVM> GetNewCourseDropdownsValues()
        {
            var response = new NewCourseDropdownsVM()
            {
                Videos = await _context.Videos
                    .OrderBy(n => n.VideoDescription)
                    .ToListAsync()
            };

            return response;
        }

        // Cập nhật thông tin khóa học
        public async Task UpdateCourseAsync(NewCourseVM data)
        {
            var dbCourse = await _context.Courses.FirstOrDefaultAsync(n => n.Id == data.Id);
            if (data.DiscountPercent == 0)
            {
                data.DiscountPercent = null;
            }
            if (dbCourse != null)
            {
                dbCourse.Name = data.Name;
                dbCourse.Description = data.Description;
                dbCourse.Price = data.Price;
                dbCourse.ImageURL = data.ImageURL;
                dbCourse.DiscountPercent = data.DiscountPercent;
                dbCourse.StartDate = data.StartDate;
                dbCourse.EndDate = data.EndDate;
                dbCourse.CourseCategory = data.CourseCategory;
                await _context.SaveChangesAsync();
            }

            // Xóa các video cũ
            var existingVideosDb = _context.Videos_Courses
                .Where(n => n.CourseId == data.Id).ToList();
            _context.Videos_Courses.RemoveRange(existingVideosDb);
            await _context.SaveChangesAsync();

            // Gắn video mới
            foreach (var videoId in data.VideoIds)
            {
                var newVideoCourse = new Video_Course()
                {
                    CourseId = data.Id,
                    VideoId = videoId
                };
                await _context.Videos_Courses.AddAsync(newVideoCourse);
            }
            await _context.SaveChangesAsync();
        }

        // Lấy toàn bộ thông tin khóa học (dùng cho trang chi tiết)
        public async Task<CourseHierarchyVM> GetCourseHierarchyAsync(int courseId, string userId, string cartId)
        {
            var course = await _context.Courses
                .Include(c => c.Sections)
                    .ThenInclude(s => s.ContentItems)
                        .ThenInclude(ci => ci.Video)
                .Include(c => c.Sections)
                    .ThenInclude(s => s.ContentItems)
                        .ThenInclude(ci => ci.Quiz)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null) return null;

            var isPurchased = _context.OrderItems
                .Any(x => x.CourseId == courseId && x.Order.UserId == userId);

            var isInCart = _context.ShoppingCartItems
                .Any(x => x.CourseId == courseId && x.ShoppingCartId == cartId);

            // ✳ Lấy tất cả QuizId từ content items
            var quizIds = course.Sections
                .SelectMany(s => s.ContentItems)
                .Where(ci => ci.Quiz != null)
                .Select(ci => ci.Quiz.Id)
                .ToList();

            // ✳ Truy vấn điểm cao nhất từ QuizResults (nếu user đã đăng nhập)
            Dictionary<int, int> quizScores = new();
            if (!string.IsNullOrEmpty(userId))
            {
                quizScores = await _context.QuizResults
                    .Where(r => quizIds.Contains(r.QuizId) && r.UserId == userId)
                    .GroupBy(r => r.QuizId)
                    .Select(g => new { QuizId = g.Key, MaxScore = g.Max(r => r.Score) })
                    .ToDictionaryAsync(g => g.QuizId, g => g.MaxScore);
            }

            return new CourseHierarchyVM
            {
                Course = course,
                Sections = course.Sections.ToList(),
                IsPurchased = isPurchased,
                IsInCart = isInCart,
                QuizHighScores = quizScores
            };
        }

        // 🔥 API TRANG CHỦ: Lấy 3 khóa học nổi bật (có giá, mới nhất)
        public async Task<IEnumerable<CourseListItemVM>> GetFeaturedCoursesAsync()
        {
            var courses = await _context.Courses
                .Where(c => c.Price > 0)
                .OrderByDescending(c => c.Id)
                .Take(3)
                .Select(c => new CourseListItemVM
                {
                    CourseId = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CoverImageUrl = c.ImageURL,
                    Tuition = c.FinalPrice,
                    Level = c.CourseCategory.ToString()
                })
                .ToListAsync();

            return courses;
        }

        public async Task<IEnumerable<CourseListItemVM>> GetRecommendedCoursesAsync(string userId, int limit = 4)
        {
            // Get purchased course IDs
            var purchasedIds = await _context.Orders
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.OrderItems.Select(oi => oi.CourseId))
                .ToListAsync();

            // Get non-purchased, recent or popular courses
            var recommendedCourses = await _context.Courses
                .Where(c => !purchasedIds.Contains(c.Id))
                .OrderByDescending(c => c.StartDate) // Or use popularity, etc.
                .Take(limit)
                .Select(c => new CourseListItemVM
                {
                    CourseId = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CoverImageUrl = c.ImageURL,
                    Tuition = c.FinalPrice,
                    Level = c.CourseCategory.ToString()
                })
                .ToListAsync();

            return recommendedCourses;
        }

    }
}

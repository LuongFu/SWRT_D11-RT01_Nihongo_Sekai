using JapaneseLearningPlatform.Data.Base;
using JapaneseLearningPlatform.Data.ViewModels;
using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearningPlatform.Data.Services
{
    public class CoursesService : EntityBaseRepository<Course>, ICoursesService
    {
        private readonly AppDbContext _context;
        public CoursesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewCourseAsync(NewCourseVM data)
        {
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
            //Add Course Videos
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

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            var courseDetails = await _context.Courses
                .Include(vc => vc.Videos_Courses).ThenInclude(a => a.Video)
                .FirstOrDefaultAsync(n => n.Id == id);

            return courseDetails;
        }

        public async Task<NewCourseDropdownsVM> GetNewCourseDropdownsValues()
        {
            var response = new NewCourseDropdownsVM()
            {
                Videos = await _context.Videos.OrderBy(n => n.VideoDescription).ToListAsync()
            };

            return response;
        }

        public async Task UpdateCourseAsync(NewCourseVM data)
        {
            var dbCourse = await _context.Courses.FirstOrDefaultAsync(n => n.Id == data.Id);

            if(dbCourse != null)
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
            //Remove existing videos
            var existingVideosDb = _context.Videos_Courses.Where(n => n.CourseId == data.Id).ToList();
            _context.Videos_Courses.RemoveRange(existingVideosDb);
            await _context.SaveChangesAsync();

            //Add Course Videos
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


    }
}

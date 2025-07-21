using JapaneseLearningPlatform.Data;               // cho AppDbContext
using JapaneseLearningPlatform.Models;             // cho CourseRating
using JapaneseLearningPlatform.Data.Services;      // cho ICourseRatingService
using Microsoft.EntityFrameworkCore;               // cho Include(), ToListAsync(), v.v.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data;
namespace JapaneseLearningPlatform.Data.Services
{

    public class CourseRatingService : ICourseRatingService
    {
        private readonly AppDbContext _context;

        public CourseRatingService(AppDbContext context)
            => _context = context;

        public async Task AddRatingAsync(string userId, int courseId, int stars, string? comment)
        {
            var rating = new CourseRating
            {
                CourseId = courseId,
                UserId = userId,
                Stars = stars,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };
            _context.CourseRatings.Add(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<(double avg, int total, Dictionary<int, int> counts)> GetStatsAsync(int courseId)
        {
            var all = await _context.CourseRatings
                                    .Where(r => r.CourseId == courseId)
                                    .ToListAsync();
            var total = all.Count;
            var avg = total > 0 ? all.Average(r => r.Stars) : 0;
            var counts = all.GroupBy(r => r.Stars)
                            .ToDictionary(g => g.Key, g => g.Count());
            return (Math.Round(avg, 1), total, counts);
        }

        // implementation
        public async Task<List<CourseRating>> GetLatestAsync(int courseId, int pageSize, int page, string sort)
        {
            var q = _context.CourseRatings
                .Where(r => r.CourseId == courseId);

            // sort
            q = sort switch
            {
                "Newest" => q.OrderByDescending(r => r.CreatedAt),
                "Oldest" => q.OrderBy(r => r.CreatedAt),
                "RatingHigh" => q.OrderByDescending(r => r.Stars),
                "RatingLow" => q.OrderBy(r => r.Stars),
                _ => q.OrderByDescending(r => r.CreatedAt)
            };

            // paging
            return await q
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(r => r.User)
                .ToListAsync();
        }
    }
}
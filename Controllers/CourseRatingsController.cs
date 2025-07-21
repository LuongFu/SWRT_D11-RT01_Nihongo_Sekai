using System.Linq;                // <<-- thêm để dùng .Select()
using System.Security.Claims;
using System.Threading.Tasks;
using JapaneseLearningPlatform.Data.Services;
using JapaneseLearningPlatform.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearningPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseRatingsController : ControllerBase
    {
        private readonly ICourseRatingService _svc;
        public CourseRatingsController(ICourseRatingService svc)
            => _svc = svc;

        public class RatingCreateVM
        {
            public int CourseId { get; set; }
            public int Stars { get; set; }
            public string Comment { get; set; }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostRating([FromBody] RatingCreateVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            await _svc.AddRatingAsync(userId, model.CourseId, model.Stars, model.Comment ?? "");
            return Ok(new { message = "Cảm ơn bạn đã gửi đánh giá!" });
        }

        [HttpGet("Stats/{courseId}")]
        public async Task<IActionResult> GetStats(int courseId)
        {
            var (avg, total, counts) = await _svc.GetStatsAsync(courseId);
            return Ok(new { avg, total, counts });
        }

        [HttpGet("Comments/{courseId}")]
        public async Task<IActionResult> GetComments(
    int courseId,
    int pageSize = 5,
    int page = 1,           // <-- thêm page
    string sort = "Newest")
        {
            // gọi service với paging + sort
            var comments = await _svc.GetLatestAsync(courseId, pageSize, page, sort);

            var dto = comments.Select(c => new {
                userName = c.User.FullName,
                avatarUrl = c.User.ProfilePicturePath,
                rating = c.Stars,
                comment = c.Comment,
                date = c.CreatedAt.ToString("dd/MM/yyyy")
            });

            return Ok(dto);
        }

    }
}

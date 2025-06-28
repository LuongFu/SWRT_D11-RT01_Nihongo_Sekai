using JapaneseLearningPlatform.Data.Services;
using Microsoft.AspNetCore.Mvc;
using JapaneseLearningPlatform.Data.ViewModels;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ICoursesService _coursesService;
        //private readonly IClassroomService _classroomService;

        public ApiController(ICoursesService coursesService/*, IClassroomService classroomService*/)
        {
            _coursesService = coursesService;
            //_classroomService = classroomService;
        }

        [HttpGet("courses/featured")]
        public async Task<IActionResult> GetFeaturedCourses()
        {
            var courses = await _coursesService.GetFeaturedCoursesAsync();
            return Ok(new { success = true, data = courses });
        }

        //[HttpGet("classrooms/popular")]
        //public async Task<IActionResult> GetPopularClassrooms()
        //{
        //    var classrooms = await _classroomService.GetPopularClassroomsAsync(3);
        //    return Ok(new { success = true, data = classrooms });
        //}
    }
}

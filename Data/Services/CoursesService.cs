using NihongoSekaiWebApplication_D11_RT01.Data.Base;
using NihongoSekaiWebApplication_D11_RT01.Data.ViewModels;
using NihongoSekaiWebApplication_D11_RT01.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NihongoSekaiWebApplication_D11_RT01.Data.Services
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
                CinemaId = data.CinemaId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                CourseCategory = data.CourseCategory,
                ProducerId = data.ProducerId
            };
            await _context.Courses.AddAsync(newCourse);
            await _context.SaveChangesAsync();

            //Add Course Actors
            foreach (var actorId in data.ActorIds)
            {
                var newActorCourse = new Actor_Course()
                {
                    CourseId = newCourse.Id,
                    ActorId = actorId
                };
                await _context.Actors_Courses.AddAsync(newActorCourse);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            var courseDetails = await _context.Courses
                .Include(c => c.Cinema)
                .Include(p => p.Producer)
                .Include(am => am.Actors_Courses).ThenInclude(a => a.Actor)
                .FirstOrDefaultAsync(n => n.Id == id);

            return courseDetails;
        }

        public async Task<NewCourseDropdownsVM> GetNewCourseDropdownsValues()
        {
            var response = new NewCourseDropdownsVM()
            {
                Actors = await _context.Actors.OrderBy(n => n.FullName).ToListAsync(),
                Cinemas = await _context.Cinemas.OrderBy(n => n.Name).ToListAsync(),
                Producers = await _context.Producers.OrderBy(n => n.FullName).ToListAsync()
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
                dbCourse.CinemaId = data.CinemaId;
                dbCourse.StartDate = data.StartDate;
                dbCourse.EndDate = data.EndDate;
                dbCourse.CourseCategory = data.CourseCategory;
                dbCourse.ProducerId = data.ProducerId;
                await _context.SaveChangesAsync();
            }

            //Remove existing actors
            var existingActorsDb = _context.Actors_Courses.Where(n => n.CourseId == data.Id).ToList();
            _context.Actors_Courses.RemoveRange(existingActorsDb);
            await _context.SaveChangesAsync();

            //Add Course Actors
            foreach (var actorId in data.ActorIds)
            {
                var newActorCourse = new Actor_Course()
                {
                    CourseId = data.Id,
                    ActorId = actorId
                };
                await _context.Actors_Courses.AddAsync(newActorCourse);
            }
            await _context.SaveChangesAsync();
        }
    }
}

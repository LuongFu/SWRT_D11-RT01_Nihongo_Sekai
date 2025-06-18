using JapaneseLearningPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Data.ViewModels
{
    public class NewCourseDropdownsVM
    {
        public NewCourseDropdownsVM()
        {
            Producers = new List<Producer>();
            Cinemas = new List<Cinema>();
            Actors = new List<Actor>();
            Videos = new List<Video>();
        }

        public List<Producer> Producers { get; set; }
        public List<Cinema> Cinemas { get; set; }
        public List<Actor> Actors { get; set; }
        public List<Video> Videos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Models
{
    public class Actor_Course
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}

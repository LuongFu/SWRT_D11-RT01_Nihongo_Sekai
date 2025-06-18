using JapaneseLearningPlatform.Data.Base;
using JapaneseLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Data.Services
{
    public class VideosService : EntityBaseRepository<Video>, IVideosService
    {
        public VideosService(AppDbContext context) : base(context) { }
    }
}

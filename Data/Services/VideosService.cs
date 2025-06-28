using JapaneseLearningPlatform.Data.Base;
using JapaneseLearningPlatform.Models;

namespace JapaneseLearningPlatform.Data.Services
{
    public class VideosService : EntityBaseRepository<Video>, IVideosService
    {
        public VideosService(AppDbContext context) : base(context) { }
    }
}

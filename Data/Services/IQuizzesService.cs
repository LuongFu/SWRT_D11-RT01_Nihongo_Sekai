using JapaneseLearningPlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JapaneseLearningPlatform.Data.Services
{

    public interface IQuizzesService
    {
        Task<Quiz> GetQuizByIdAsync(int id);
        Task<IEnumerable<Quiz>> GetAllAsync();
        Task AddAsync(Quiz quiz);
        Task UpdateAsync(Quiz quiz);
        Task DeleteAsync(int id);
    }

}

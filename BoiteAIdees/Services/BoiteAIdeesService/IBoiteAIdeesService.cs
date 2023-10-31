using BoiteAIdees.Models;

namespace BoiteAIdees.Services.BoiteAIdeesService
{
    public interface IBoiteAIdeesService
    {
        Task<List<Ideas>> GetAllIdeas();
        Task<Ideas?> GetIdeaById(int id);
    }
}

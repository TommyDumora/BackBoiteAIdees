using BoiteAIdees.Models.Domaine;

namespace BoiteAIdees.Services.BoiteAIdeesService
{
    /// <summary>
    /// Interface pour le service BoiteAIdees.
    /// </summary>
    public interface IBoiteAIdeesService
    {

        /// <summary>
        /// Récupère toutes les idées.
        /// </summary>
        Task<List<Ideas>> GetAllIdeas();

        /// <summary>
        /// Récupère une idée par son identifiant.
        /// </summary>
        Task<Ideas> GetIdeaById(int id);

        /// <summary>
        /// Création d'une idée.
        /// </summary>
        Task<Ideas> AddIdea(Ideas newIdea);
    }
}
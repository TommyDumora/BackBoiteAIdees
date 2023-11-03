using BoiteAIdees.Context;
using BoiteAIdees.Models.Domaine;
using Microsoft.EntityFrameworkCore;

namespace BoiteAIdees.Services.BoiteAIdeesService
{
    /// <summary>
    /// Implémentation du service BoiteAIdees.
    /// </summary>
    public class BoiteAIdeesService : IBoiteAIdeesService
    {
        private readonly BoiteAIdeesContext _context;

        /// <summary>
        /// Constructeur de la classe BoiteAIdeesService.
        /// </summary>
        /// <param name="context">Contexte de la base de données BoiteAIdees.</param>
        public BoiteAIdeesService(BoiteAIdeesContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère toutes les idées.
        /// </summary>
        /// <returns>Liste d'idées.</returns>
        public async Task<List<Ideas>> GetAllIdeas()
        {
            return await _context.Ideas
                .Include(i => i.Category)
                .Include(i => i.User)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère une idée par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'idée.</param>
        /// <returns>Une idée spécifique ou null si non trouvée.</returns>
        public async Task<Ideas> GetIdeaById(int id)
        {
            if (id <= 0)  throw new ArgumentException("L'identifiant de l'idée doit être supérieur à zéro.");

            var idea = await _context.Ideas
                .Include(i => i.Category)
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.IdeaId == id);

            return idea ?? throw new InvalidOperationException("L'idée avec cet identifiant est introuvable.");
        }

        /// <summary>
        /// Permet de créer une idée.
        /// </summary>
        /// <param name="newIdea">Représente une idée.</param>
        /// <returns>Une idée est ajoutée.</returns>
        public async Task<Ideas> AddIdea(Ideas newIdea)
        {
            if (newIdea == null) throw new ArgumentNullException(nameof(newIdea), "L'idée à ajouter est nulle.");

            _context.Ideas.Add(newIdea);

            await _context.Entry(newIdea)
                .Reference(i => i.Category)
                .LoadAsync();

            await _context.Entry(newIdea)
                .Reference(i => i.User)
                .LoadAsync();

            await _context.SaveChangesAsync();

            return newIdea;
        }

        /// <summary>
        /// Permet de supprimer une idée.
        /// </summary>
        /// <param name="id">Identifiant de l'idée.</param>
        /// <returns>Une idée est supprimé.</returns>
        public async Task DeleteIdea(int id)
        {
            var idea = await GetIdeaById(id);

            if (idea != null)
            {
                _context.Ideas.Remove(idea);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Permet de mettre à jour une idée.
        /// </summary>
        /// <param name="updateIdea">Représente une idée.</param>
        /// <returns>Une idée est modifié.</returns>
        public async Task<Ideas> UpdateIdea(Ideas updateIdea)
        {
            if (updateIdea == null) throw new ArgumentNullException(nameof(updateIdea), "L'idée à mettre à jour est nulle.");

            updateIdea.UpdatedAt = DateTime.Now;
            _context.Entry(updateIdea).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updateIdea;
        }
    }
}

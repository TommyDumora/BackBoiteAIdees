using BoiteAIdees.Context;
using BoiteAIdees.Models.Domaine;
using Microsoft.EntityFrameworkCore;

namespace BoiteAIdees.Services
{
    /// <summary>
    /// Service de gestion des catégories.
    /// </summary>
    public class CategoriesService
    {
        private readonly BoiteAIdeesContext _context;

        /// <summary>
        /// Constructeur de la classe CategoriesService.
        /// </summary>
        /// <param name="context">Contexte de la base de données BoiteAIdees.</param>
        public CategoriesService(BoiteAIdeesContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère toutes les catégories.
        /// </summary>
        /// <returns>Liste de catégories.</returns>
        public async Task<List<Categories>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Categories> GetCategorieById(int id)
        {
            if (id <= 0) throw new ArgumentException("L'identifiant de la catégorie doit être supérieur à zéro.");

            var categorie = await _context.Categories.FindAsync(id);

            return categorie ?? throw new InvalidOperationException("La catégorie avec cet identifiant est introuvable.");
        }

        public async Task<Categories> AddCategorie(Categories newCategorie)
        {
            if (newCategorie == null) throw new ArgumentNullException(nameof(newCategorie), "La catégorie ajouté est nulle");

            _context.Categories.Add(newCategorie);

            await _context.SaveChangesAsync();

            return newCategorie;
        }

        public async Task DeleteCategorie(int id)
        {
            var categorie = await GetCategorieById(id);

            if (categorie != null)
            {
                _context.Categories.Remove(categorie);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Categories> UptadeCategorie(Categories updateCategorie)
        {
            if (updateCategorie == null) throw new ArgumentNullException(nameof(updateCategorie), "La catégorie à mettre à jour est nulle.");

            _context.Entry(updateCategorie).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updateCategorie;
        }
    }
}

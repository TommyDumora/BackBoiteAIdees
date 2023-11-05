using Swashbuckle.AspNetCore.Annotations;

namespace BoiteAIdees.Models.DTOs
{
    /// <summary>
    /// Représente une catégorie au format DTO (Data Transfer Object).
    /// </summary>
    [SwaggerSchema(Description = "Modèle représentant une categorie pour l'API.")]
    public class CategoriesDto
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de la catégorie.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de la catégorie.
        /// </summary>
        public string? Name { get; set; }
    }
}

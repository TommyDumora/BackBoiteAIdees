using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BoiteAIdees.Models.DTOs
{
    /// <summary>
    /// Représente une idée au format DTO (Data Transfer Object).
    /// </summary>
    [SwaggerSchema(Description = "Modèle représentant une idée pour l'API.")]
    public class IdeasDto
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de l'idée.
        /// </summary>
        public int IdeaId { get; set; }

        /// <summary>
        /// Obtient ou définit le titre de l'idée.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Obtient ou définit la description de l'idée.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de la catégorie associée à l'idée.
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Obtient la date de la création de l'idée.
        /// </summary>
        public string? CreatedAt { get; set; }

        /// <summary>
        /// Obtient le prénom de l'utilisateur.
        /// </summary>
        public string? UserFirstName { get; set; }

        /// <summary>
        /// Obtient le nom de l'utilisateur.
        /// </summary>
        public string? UserLastName { get; set; }
    }

    /// <summary>
    /// Créé une idée au format DTO (Data Transfer Object).
    /// </summary>
    [SwaggerSchema(Description = "Modèle représentant l'ajout d'une idée dans l'API.")]
    public class CreateIdeasDto
    {
        /// <summary>
        /// Obtient ou définit le titre de l'idée.
        /// </summary>
        [Required(ErrorMessage = "Un titre est requis")]
        public string? Title { get; set; }

        /// <summary>
        /// Obtient ou définit la description de l'idée.
        /// </summary>
        [Required(ErrorMessage = "Une description est requise")]
        public string? Description { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la catégorie associée à l'idée.
        /// </summary>
        [Required(ErrorMessage = "Id de la categorie à associée à l'idée")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur associé à l'idée.
        /// </summary>
        [Required(ErrorMessage = "Id de l'utilisateur à associée à l'idée")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// Modifie une idée au format DTO (Data Transfer Object).
    /// </summary>
    [SwaggerSchema(Description = "Modèle représentant la mise à jour d'une idée pour l'API.")]
    public class UpdateIdea
    {
        /// <summary>
        /// Obtient ou définit le titre de l'idée.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Obtient ou définit la description de l'idée.
        /// </summary>
        public string? Description { get; set; }
    }
}

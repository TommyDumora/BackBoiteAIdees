using System.ComponentModel.DataAnnotations;

namespace BoiteAIdees.Models.Domaine
{
    /// <summary>
    /// Représente une idée.
    /// </summary>
    public partial class Ideas
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
        /// Obtient ou définit la date de création de l'idée.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Obtient ou définit la date de mise à jour de l'idée.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la catégorie associée à l'idée.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur associé à l'idée.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Obtient ou définit la catégorie associée à l'idée.
        /// </summary>
        public virtual Categories? Category { get; set; }

        /// <summary>
        /// Obtient ou définit l'utilisateur associé à l'idée.
        /// </summary>
        public virtual Users? User { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des utilisateurs ayant aimé cette idée.
        /// </summary>
        public virtual ICollection<UserLikedIdeas> UserLikedIdeas { get; set; } = new List<UserLikedIdeas>();
    }
}

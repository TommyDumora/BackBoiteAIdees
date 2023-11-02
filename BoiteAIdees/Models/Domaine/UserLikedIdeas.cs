namespace BoiteAIdees.Models.Domaine
{
    /// <summary>
    /// Représente une relation entre l'utilisateur et les idées aimées.
    /// </summary>
    public partial class UserLikedIdeas
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'idée aimée.
        /// </summary>
        public int IdeaId { get; set; }

        /// <summary>
        /// Obtient ou définit l'idée aimée associée à cet enregistrement.
        /// </summary>
        public virtual Ideas? Idea { get; set; }

        /// <summary>
        /// Obtient ou définit l'utilisateur associé à cet enregistrement.
        /// </summary>
        public virtual Users? User { get; set; }
    }
}

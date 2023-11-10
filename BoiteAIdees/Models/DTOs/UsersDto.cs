using BoiteAIdees.Models.Domaine;

namespace BoiteAIdees.Models.DTOs
{
    public class UsersDto
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom de l'utilisateur.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de l'utilisateur.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Obtient ou définit l'adresse e-mail de l'utilisateur.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Obtient ou définit le hachage du mot de passe de l'utilisateur.
        /// </summary>
        // public string? PasswordHash { get; set; }

        /// <summary>
        /// Obtient ou définit si l'utilisateur est administrateur.
        /// </summary>
        // public bool IsAdmin { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des idées associées à cet utilisateur.
        /// </summary>
        // public virtual ICollection<Ideas> Ideas { get; set; } = new List<Ideas>();

        /// <summary>
        /// Obtient ou définit la liste des idées aimées par cet utilisateur.
        /// </summary>
        // public virtual ICollection<UserLikedIdeas> UserLikedIdeas { get; set; } = new List<UserLikedIdeas>();
    }

    public class AddUserDto
    {
        /// <summary>
        /// Obtient ou définit le prénom de l'utilisateur.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de l'utilisateur.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Obtient ou définit l'adresse e-mail de l'utilisateur.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Obtient ou définit le hachage du mot de passe de l'utilisateur.
        /// </summary>
        public string? PasswordHash { get; set; }
    }
}

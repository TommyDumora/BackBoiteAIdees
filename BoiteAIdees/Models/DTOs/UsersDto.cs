using System.ComponentModel.DataAnnotations;

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
        public string? Password { get; set; }
    }

    public class AddUserDto
    {
        /// <summary>
        /// Obtient ou définit le prénom de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le prénom est requis.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le nom est requis.")]
        public string LastName { get; set; }

        /// <summary>
        /// Obtient ou définit l'adresse e-mail de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        public string Email { get; set; }

        /// <summary>
        /// Obtient ou définit le hachage du mot de passe de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        public string Password { get; set; }
    }

    public class UsersLogin
    {
        /// <summary>
        /// Obtient ou définit l'adresse e-mail de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        public string Email { get; set; }

        /// <summary>
        /// Obtient ou définit le hachage du mot de passe de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        public string Password { get; set; }

    }
}

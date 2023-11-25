using BoiteAIdees.Context;
using BoiteAIdees.Models.Domaine;
using Microsoft.EntityFrameworkCore;

namespace BoiteAIdees.Services
{
    public class UsersService
    {
        private readonly BoiteAIdeesContext _context;

        public UsersService(BoiteAIdeesContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> GetUserById(int id)
        {
            if (id <= 0) throw new ArgumentException("L'identifiant de l'utilisateur doit être supérieur à zéro.");

            var user = await _context.Users.FindAsync(id);

            return user ?? throw new InvalidOperationException("L'utilisateur avec cet identifiant est introuvable.");
        }

        public async Task<Users> AddUser(Users model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model), "L'utilisateur à ajouter est nulle.");

            if (!IsPasswordStrong(model.PasswordHash)) throw new ArgumentException("Le mot de passe ne répond pas aux critères de sécurité.");

            model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);

            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task DeleteUser(int id)
        {
            var user = await GetUserById(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                return false;
            }

            bool containsUppercase = password.Any(char.IsUpper);
            bool containsLowercase = password.Any(char.IsLower);
            bool containsDigit = password.Any(char.IsDigit);

            return containsUppercase && containsLowercase && containsDigit;
        }

        public async Task<Users?> GetUserByEmail(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) throw new ArgumentException("L'adresse e-mail et le mot de passe sont requis.");

            var users = await _context.Users.Where(u => u.Email == email).ToListAsync();

            foreach (var user in users)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return user;
                }
            }

            return null;
        }
    }
}
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

        public async Task<Users> AddUser(Users newUser)
        {
            if (newUser == null) throw new ArgumentNullException(nameof(newUser), "L'utilisateur à ajouter est nulle.");

            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();

            return newUser;
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
    }
}

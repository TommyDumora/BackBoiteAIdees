using BoiteAIdees.Context;
using BoiteAIdees.Models.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BoiteAIdees.Services
{
    public class AuthService
    {
        private readonly BoiteAIdeesContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(BoiteAIdeesContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        internal string CreateToken(Users user)
        {
            List<Claim> claims = new()
            {
                new Claim("userId", user.UserId.ToString())
            };

            if (user.IsAdmin)
            {
                claims.Add(new Claim("role", "Admin"));
            }
            else
            {
                claims.Add(new Claim("role", "User"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}

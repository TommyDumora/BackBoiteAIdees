using BoiteAIdees.Models.DTOs;
using BoiteAIdees.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoiteAIdees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsersDto>> Login(UsersLogin request)
        {
            try
            {
                var existingUser = await _authService.GetUserByEmail(request.Email, request.Password);

                if (existingUser == null) return BadRequest($"L'utilisateur avec l'email {request.Email} n'a pas été trouvé.");

                if (!BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash)) return BadRequest("Mauvais mot de passe");

                string token = _authService.CreateToken(existingUser);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur : " + ex.Message);
            }
        }
    }
}

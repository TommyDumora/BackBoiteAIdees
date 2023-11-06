using BoiteAIdees.Models.Domaine;
using BoiteAIdees.Models.DTOs;
using BoiteAIdees.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoiteAIdees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UsersService _service;

        public UsersController(UsersService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsersDto>>> GetAllUsers()
        {
            try
            {
                var users = await _service.GetAllUsers();

                if (users == null || !users.Any()) return NotFound("Acun utilisateurs trouvée");

                List<UsersDto> usersDtos = users.Select(u => new UsersDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }).ToList();

                return Ok(usersDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur : " + ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsersDto>> GetUserById([FromRoute] int id)
        {
            try
            {
                var user = await _service.GetUserById(id);

                if (user == null) return NotFound($"L'utilisateur avec l'id {id} n'a pas été trouvée.");

                UsersDto userDto = new()
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };

                return Ok(userDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Erreur dans l'argument : " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound("Erreur lors de la récupération de l'utilisateur : " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur : " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsersDto>> CreateUser([FromBody] AddUserDto model)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                Users newUser = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash),
                };

                await _service.AddUser(newUser);

                UsersDto userDto = new()
                {
                    UserId = newUser.UserId,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                };

                return CreatedAtAction(nameof(GetUserById), new { id = userDto.UserId }, userDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Erreur dans l'argument : " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur : " + ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                var existingUser = await _service.GetUserById(id);

                if (existingUser == null) return NotFound($"L'utilisateur avec l'id {id} n'a pas été trouvée.");

                await _service.DeleteUser(id);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Erreur dans l'argument : " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur : " + ex.Message);
            }
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using BoiteAIdees.Services;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace BoiteAIdees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLikedIdeasController : ControllerBase
    {
        private readonly UserLikedIdeasService _likeService;

        public UserLikedIdeasController(UserLikedIdeasService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("{id:int}/like"), Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> LikeIdea(int id)
        {
            // Récupérez le JWT depuis la requête (vous pouvez utiliser [Authorize] pour obtenir automatiquement l'identité de l'utilisateur)
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Décodage du JWT pour obtenir les revendications (y compris userId)
            var handler = new JwtSecurityTokenHandler();

            // Vérifier que le décodage du JWT n'est pas null
            if (handler.ReadToken(token) is not JwtSecurityToken jsonToken)
            {
                return Unauthorized("Token JWT invalide.");
            }

            // Obtenez l'ID de l'utilisateur à partir du JWT
            var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "userId");

            // Vérifier que la revendication userId est présente
            if (userIdClaim == null)
            {
                return Unauthorized("Revendication 'userId' manquante dans le token JWT.");
            }

            var userId = int.Parse(userIdClaim.Value);


            var like = await _likeService.LikeIdea(userId, id);

            if (like == null)
            {
                return NoContent();
            }

            return NoContent();
        }
    }
}

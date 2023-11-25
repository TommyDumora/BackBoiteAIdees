using Microsoft.AspNetCore.Mvc;
using BoiteAIdees.Services;

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

        [HttpPost("{id:int}/like")]
        public async Task<ActionResult> LikeIdea(int id)
        {
            //var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userId = 1;

            var like = await _likeService.LikeIdea(userId, id);

            if (like == null)
            {
                return NoContent();
            }

            return NoContent();
        }
    }
}

using BoiteAIdees.Context;
using BoiteAIdees.Models.Domaine;
using Microsoft.EntityFrameworkCore;

namespace BoiteAIdees.Services
{
    // UserLikedIdeasService.cs
    public class UserLikedIdeasService
    {
        private readonly BoiteAIdeesContext _context;

        public UserLikedIdeasService(BoiteAIdeesContext context)
        {
            _context = context;
        }

        public async Task<UserLikedIdeas> LikeIdea(int userId, int ideaId)
        {
            var existingLike = await _context.UserLikedIdeas.FirstOrDefaultAsync(u => u.UserId == userId && u.IdeaId == ideaId);

            if (existingLike == null)
            {
                var like = new UserLikedIdeas { UserId = userId, IdeaId = ideaId };
                _context.UserLikedIdeas.Add(like);
                await _context.SaveChangesAsync();
                return like;
            }

            return null; // L'utilisateur a déjà liké cette idée
        }

        public async Task<UserLikedIdeas> DislikeIdea(int userId, int ideaId)
        {
            var existingLike = await _context.UserLikedIdeas.FirstOrDefaultAsync(u => u.UserId == userId && u.IdeaId == ideaId);

            if (existingLike == null)
            {
                var dislike = new UserLikedIdeas { UserId = userId, IdeaId = ideaId };
                _context.UserLikedIdeas.Add(dislike);
                await _context.SaveChangesAsync();
                return dislike;
            }

            return null; // L'utilisateur a déjà disliké cette idée
        }
    }

}

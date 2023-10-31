using BoiteAIdees.Context;
using BoiteAIdees.Models;
using Microsoft.EntityFrameworkCore;

namespace BoiteAIdees.Services.BoiteAIdeesService
{
    public class BoiteAIdeesService : IBoiteAIdeesService
    {
        private readonly BoiteAIdeesContext _context;

        public BoiteAIdeesService(BoiteAIdeesContext context)
        {
            _context = context;
        }

        public async Task<List<Ideas>> GetAllIdeas()
        {
            return await _context.Ideas.ToListAsync();
        }

        public async Task<Ideas?> GetIdeaById(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return null;
            }
            else
            {
                return idea;
            }
        }
    }
}

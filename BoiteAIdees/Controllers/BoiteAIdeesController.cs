using BoiteAIdees.Models;
using BoiteAIdees.Services.BoiteAIdeesService;
using Microsoft.AspNetCore.Mvc;

namespace BoiteAIdees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoiteAIdeesController : ControllerBase
    {
        private readonly IBoiteAIdeesService _service;

        public BoiteAIdeesController(IBoiteAIdeesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ideas>>> GetAllIdeas()
        {
            try
            {
                var ideas = await _service.GetAllIdeas();

                if (ideas == null)
                {
                    return NotFound("Aucune idée trouvée.");
                }

                return Ok(ideas);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur s'est produite lors du traitement de la requête.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ideas>> GetIdeaById(int id)
        {
            try
            {
                var idea = await _service.GetIdeaById(id);

                if (idea == null)
                {
                    return NotFound("Aucune idée trouvée pour l'ID spécifié.");
                }

                return Ok(idea);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur s'est produite lors du traitement de la requête.");
            }
        }
    }
}

using BoiteAIdees.Models.Domaine;
using BoiteAIdees.Models.DTOs;
using BoiteAIdees.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace BoiteAIdees.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des idées.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly IdeasService _service;

        /// <summary>
        /// Constructeur du contrôleur BoiteAIdees.
        /// </summary>
        /// <param name="service">Service BoiteAIdees pour la gestion des idées.</param>
        public IdeasController(IdeasService service)
        {
            _service = service;
        }

        /// <summary>
        /// Récupère toutes les idées
        /// </summary>
        /// <response code="200">Retourne la liste de toutes les idées</response>
        /// <response code="404">Aucune idée trouvée.</response>
        /// <response code="500">Une erreur s'est produite lors du traitement de la requête.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Retourne toutes les idées",
            Description = "Retourne la liste de toutes les idées.",
            OperationId = "GetAllIdeas"
        )]
        [SwaggerResponse(200, "Toutes les idées ont été trouvées avec succès.", typeof(List<IdeasDto>))]
        [SwaggerResponse(404, "Aucune idée trouvée.")]
        [SwaggerResponse(500, "Une erreur s'est produite lors du traitement de la requête.")]
        public async Task<ActionResult<List<IdeasDto>>> GetAllIdeas()
        {
            try
            {
                var ideas = await _service.GetAllIdeas();

                if (ideas == null || !ideas.Any()) return NotFound("Aucune idée trouvée.");

                List<IdeasDto> ideasDtos = ideas.Select(i => _service.MapToIdeasDto(i)).ToList();

                return Ok(ideasDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur : " + ex.Message);
            }
        }

        /// <summary>
        /// Récupère une idée par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'idée est un chiffre entier.</param>
        /// <response code="200">Retourne une idées en fonction de sont ID</response>
        /// <response code="404">Aucune idée trouvée.</response>
        /// <response code="500">Une erreur s'est produite lors du traitement de la requête.</response>
        /// <returns>Une idée spécifique ou null si non trouvée.</returns>
        [HttpGet("{id:int}")]
        [SwaggerOperation(
        Summary = "Retourne un idée",
        Description = "Prend un ID en entrée pour renvoyer une idée en fonction.",
        OperationId = "GetIdeaById"
        )]
        [SwaggerResponse(200, "L'idée a bien été trouvée", typeof(IdeasDto))]
        [SwaggerResponse(404, "Aucune idée trouvée pour l'ID spécifié.")]
        [SwaggerResponse(500, "Une erreur s'est produite lors du traitement de la requête.")]
        public async Task<ActionResult<IdeasDto>> GetIdeaById([FromRoute] int id)
        {
            try
            {
                var idea = await _service.GetIdeaById(id);

                if (idea == null) return NotFound($"L'idée avec l'id {id} n'a pas été trouvée.");

                IdeasDto ideasDto = _service.MapToIdeasDto(idea);

                return Ok(ideasDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Erreur dans l'argument : " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound("Erreur lors de la récupération de l'idée : " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur : " + ex.Message);
            }
        }

        /// <summary>
        /// Permet de créer une idée.
        /// </summary>
        /// <param name="model">Représente une idée.</param>
        /// <response code="201">L'idée a été créée avec succès.</response>
        /// <response code="400">Requête incorrecte.</response>
        /// <response code="500">Une erreur s'est produite lors du traitement de la requête.</response>
        [HttpPost, Authorize(Roles = "Admin,User")]
        [SwaggerOperation(
        Summary = "Permet de créer une idée",
        Description = "Permet de créer une idée et de l'ajouter en base de donnée.",
        OperationId = "CreateIdea"
        )]
        [SwaggerResponse(201, "L'idée a été créée avec succès.", typeof(IdeasDto))]
        [SwaggerResponse(400, "Requête incorrecte.")]
        [SwaggerResponse(500, "Une erreur s'est produite lors du traitement de la requête.")]
        public async Task<ActionResult<IdeasDto>> CreateIdea([FromBody] CreateIdeasDto model)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                var handler = new JwtSecurityTokenHandler();

                if (handler.ReadToken(token) is not JwtSecurityToken jsonToken)
                {
                    return Unauthorized("Token JWT invalide.");
                }

                var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "userId");

                if (userIdClaim == null)
                {
                    return Unauthorized("Revendication 'userId' manquante dans le token JWT.");
                }

                var userId = int.Parse(userIdClaim.Value);

                Ideas newIdea = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    UserId = model.UserId,
                };

                await _service.AddIdea(userId, newIdea);

                IdeasDto ideasDto = new()
                {
                    IdeaId = newIdea.IdeaId,
                    Title = newIdea.Title,
                    Description = newIdea.Description,
                    CategoryId = newIdea.CategoryId,
                    CategoryName = newIdea.Category?.Name,
                    CreatedAt = newIdea.CreatedAt.ToString("dd MMMM yyyy HH:mm:ss", new CultureInfo("fr-FR")),
                    UserFirstName = newIdea.User?.FirstName,
                    UserLastName = newIdea.User?.LastName
                };

                return CreatedAtAction(nameof(GetIdeaById), new { id = ideasDto.IdeaId }, ideasDto);
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

        /// <summary>
        /// Permet de supprimer une idée.
        /// </summary>
        /// <param name="id">Représente l'identifiant d'une idée.</param>
        /// <response code="204">L'idée a été supprimé avec succès.</response>
        /// <response code="400">Requête incorrecte.</response>
        /// <response code="500">Une erreur s'est produite lors du traitement de la requête.</response>
        [HttpDelete("{id:int}"), Authorize(Roles = "Admin,User")]
        [SwaggerOperation(
        Summary = "Permet de supprimer une idée",
        Description = "Permet de supprimer une idée et de la retirer de la base de donnée.",
        OperationId = "DeleteIdea"
        )]
        [SwaggerResponse(204, "L'idée a été supprimé avec succès.")]
        [SwaggerResponse(400, "Requête incorrecte.")]
        [SwaggerResponse(500, "Une erreur s'est produite lors du traitement de la requête.")]
        public async Task<ActionResult> DeleteIdea(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                var handler = new JwtSecurityTokenHandler();

                if (handler.ReadToken(token) is not JwtSecurityToken jsonToken)
                {
                    return Unauthorized("Token JWT invalide.");
                }

                var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "userId");

                if (userIdClaim == null)
                {
                    return Unauthorized("Revendication 'userId' manquante dans le token JWT.");
                }

                var userId = int.Parse(userIdClaim.Value);

                var existingIdea = await _service.GetIdeaById(id);

                if (existingIdea == null)
                {
                    return NotFound($"L'idée avec l'id {id} n'a pas été trouvée.");
                }

                await _service.DeleteIdea(userId, id);

                return NoContent(); // Réponse 204 (No Content) requête traitée avec succès mais pas d’information à renvoyer.
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

        /// <summary>
        /// Permet de mettre à jour une idée.
        /// </summary>
        /// <param name="id">Représente l'identifiant d'une idée.</param>
        /// <param name="ideaDto">Représente une idée.</param>
        /// <response code="204">L'idée a été modifié avec succès.</response>
        /// <response code="400">Requête incorrecte.</response>
        /// <response code="500">Une erreur s'est produite lors du traitement de la requête.</response>
        [HttpPut("{id:int}"), Authorize(Roles = "Admin,User")]
        [SwaggerOperation(
        Summary = "Permet de mettre à jour une idée",
        Description = "Permet de mettre à jour une idée .",
        OperationId = "UpdateIdea"
        )]
        [SwaggerResponse(204, "L'idée a été modifié avec succès.")]
        [SwaggerResponse(400, "Requête incorrecte.")]
        [SwaggerResponse(500, "Une erreur s'est produite lors du traitement de la requête.")]
        public async Task<ActionResult> UpdateIdea(int id, [FromBody] UpdateIdeaDto ideaDto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                var handler = new JwtSecurityTokenHandler();

                if (handler.ReadToken(token) is not JwtSecurityToken jsonToken)
                {
                    return Unauthorized("Token JWT invalide.");
                }

                var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "userId");

                if (userIdClaim == null)
                {
                    return Unauthorized("Revendication 'userId' manquante dans le token JWT.");
                }

                var userId = int.Parse(userIdClaim.Value);

                var existingIdea = await _service.GetIdeaById(id);

                if (existingIdea == null)
                {
                    return NotFound("L'idée n'a pas été trouvée.");
                }

                existingIdea.Title = ideaDto.Title;
                existingIdea.Description = ideaDto.Description;
                existingIdea.CategoryId = ideaDto.CategoryId;

                await _service.UpdateIdea(userId, existingIdea);

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

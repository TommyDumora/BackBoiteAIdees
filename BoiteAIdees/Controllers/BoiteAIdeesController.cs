﻿using BoiteAIdees.Models.Domaine;
using BoiteAIdees.Models.DTOs;
using BoiteAIdees.Services.BoiteAIdeesService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;

namespace BoiteAIdees.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des idées.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BoiteAIdeesController : ControllerBase
    {
        private readonly IBoiteAIdeesService _service;
        private readonly ILogger<BoiteAIdeesController> _logger;

        /// <summary>
        /// Constructeur du contrôleur BoiteAIdees.
        /// </summary>
        /// <param name="service">Service BoiteAIdees pour la gestion des idées.</param>
        /// <param name="logger">Logger pour la journalisation des événements.</param>
        public BoiteAIdeesController(IBoiteAIdeesService service, ILogger<BoiteAIdeesController> logger)
        {
            _service = service;
            _logger = logger;
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

                IEnumerable<IdeasDto> ideasDto = ideas.Select(i => new IdeasDto
                {
                    IdeaId = i.IdeaId,
                    Title = i.Title,
                    Description = i.Description,
                    CategoryName = i.Category?.Name,
                    CreatedAt = i.CreatedAt.ToString("dd MMMM yyyy HH:mm:ss", new CultureInfo("fr-FR")),
                    UserFirstName = i.User?.FirstName,
                    UserLastName = i.User?.LastName
                });

                return Ok(ideasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur s'est produite lors de la récupération des idées.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur.");
            }
        }

        /// <summary>
        /// Récupère une idée par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'idée est un chiffre entier.</param>
        /// <response code="200">Retourne une idées en fonction de sont ID</response>
        /// <response code="404">Aucune idée trouvée.</response>
        /// <response code="500">Une erreur s'est produite lors du traitement de la requête.</response>
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

                IdeasDto ideasDto = new()
                {
                    IdeaId = id,
                    Title = idea.Title,
                    Description = idea.Description,
                    CategoryName = idea.Category?.Name,
                    CreatedAt = idea.CreatedAt.ToString("dd MMMM yyyy HH:mm:ss", new CultureInfo("fr-FR")),
                    UserFirstName = idea.User?.FirstName,
                    UserLastName = idea.User?.LastName
                };

                return Ok(ideasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur s'est produite lors de la récupération de l'idée.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur.");
            }
        }

        /// <summary>
        /// Permet de créer une idée.
        /// </summary>
        /// <param name="model">Représente une idée.</param>
        /// <response code="201">L'idée a été créée avec succès.</response>
        /// <response code="400">Requête incorrecte.</response>
        /// <response code="500">Une erreur s'est produite lors du traitement de la requête.</response>
        [HttpPost]
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
                Ideas newIdea = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    UserId = model.UserId,
                };

                await _service.AddIdea(newIdea);

                IdeasDto ideasDto = new()
                {
                    IdeaId = newIdea.IdeaId,
                    Title = newIdea.Title,
                    Description = newIdea.Description,
                    CategoryName = newIdea.Category?.Name,
                    CreatedAt = newIdea.CreatedAt.ToString("dd MMMM yyyy HH:mm:ss", new CultureInfo("fr-FR")),
                    UserFirstName = newIdea.User?.FirstName,
                    UserLastName = newIdea.User?.LastName
                };

                return CreatedAtAction(nameof(GetIdeaById), new { id = ideasDto.IdeaId }, ideasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création d'une nouvelle idée.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur.");
            }
        }

        /// <summary>
        /// Permet de supprimer une idée.
        /// </summary>
        /// <param name="id">Représente l'identifiant d'une idée.</param>
        /// <response code="204">L'idée a été supprimé avec succès.</response>
        /// <response code="400">Requête incorrecte.</response>
        /// <response code="500">Une erreur s'est produite lors du traitement de la requête.</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(
        Summary = "Permet de supprimer une idée",
        Description = "Permet de supprimer une idée et de la retirer de la base de donnée.",
        OperationId = "DeleteIdea"
        )]
        [SwaggerResponse(204, "L'idée a été supprimé avec succès.", typeof(ActionResult))]
        [SwaggerResponse(400, "Requête incorrecte.")]
        [SwaggerResponse(500, "Une erreur s'est produite lors du traitement de la requête.")]
        public async Task<ActionResult<bool>> DeleteIdea(int id)
        {
            try
            {
                var existingIdea = await _service.GetIdeaById(id);

                if (existingIdea == null)
                {
                    return NotFound($"L'idée avec l'id {id} n'a pas été trouvée.");
                }

                await _service.DeleteIdea(existingIdea);

                return NoContent(); // Réponse 204 (No Content) requête traitée avec succès mais pas d’information à renvoyer.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'idée.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur lors de la suppression de l'idée.");
            }
        }
    }
}

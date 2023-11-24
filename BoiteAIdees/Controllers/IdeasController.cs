﻿using BoiteAIdees.Models.Domaine;
using BoiteAIdees.Models.DTOs;
using BoiteAIdees.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using System.Security.Claims;

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
        private readonly UserLikedIdeasService _likeService;

        /// <summary>
        /// Constructeur du contrôleur BoiteAIdees.
        /// </summary>
        /// <param name="service">Service BoiteAIdees pour la gestion des idées.</param>
        public IdeasController(IdeasService service, UserLikedIdeasService likeService)
        {
            _service = service;
            _likeService = likeService;
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
        [HttpDelete("{id:int}")]
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
                var existingIdea = await _service.GetIdeaById(id);

                if (existingIdea == null)
                {
                    return NotFound($"L'idée avec l'id {id} n'a pas été trouvée.");
                }

                await _service.DeleteIdea(id);

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
        [HttpPut("{id:int}")]
        [SwaggerOperation(
        Summary = "Permet de mettre à jour une idée",
        Description = "Permet de supprimer une idée et de la retirer de la base de donnée.",
        OperationId = "UpdateIdea"
        )]
        [SwaggerResponse(204, "L'idée a été modifié avec succès.")]
        [SwaggerResponse(400, "Requête incorrecte.")]
        [SwaggerResponse(500, "Une erreur s'est produite lors du traitement de la requête.")]
        public async Task<ActionResult> UpdateIdea(int id, [FromBody] UpdateIdeaDto ideaDto)
        {
            try
            {
                var existingIdea = await _service.GetIdeaById(id);

                if (existingIdea == null)
                {
                    return NotFound("L'idée n'a pas été trouvée.");
                }

                existingIdea.Title = ideaDto.Title;
                existingIdea.Description = ideaDto.Description;
                existingIdea.CategoryId = ideaDto.CategoryId;

                await _service.UpdateIdea(existingIdea);

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

        [HttpPost("{id:int}/like")]
        public async Task<ActionResult> LikeIdea(int id)
        {
            //var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userId = 1;

            var like = await _likeService.LikeIdea(userId, id);

            if (like == null)
            {
                return BadRequest("L'utilisateur a déjà liké cette idée.");
            }

            return NoContent();
        }

        [HttpPost("{id:int}/dislike")]
        public async Task<ActionResult> DislikeIdea(int id)
        {
            //var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userId = 1;

            var dislike = await _likeService.DislikeIdea(userId, id);

            if (dislike == null)
            {
                return BadRequest("L'utilisateur a déjà disliké cette idée.");
            }

            return NoContent();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoiteAIdees.Models.Domaine;
using BoiteAIdees.Services;
using BoiteAIdees.Models.DTOs;
using System.Globalization;

namespace BoiteAIdees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesService _service;

        public CategoriesController(CategoriesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriesDto>>> GetAllCategories()
        {
            try
            {
                var categories = await _service.GetAllCategories();

                if (categories == null || !categories.Any()) return NotFound("Aucune catégorie trouvé.");

                List<CategoriesDto> categoriesDtos = categories.Select(c => new CategoriesDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                }).ToList();

                return Ok(categoriesDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur : " + ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriesDto>> GetCategorieById([FromRoute] int id)
        {
            try
            {
                var categorie = await _service.GetCategorieById(id);

                if (categorie == null) return NotFound($"La catégorie avec l'id {id} n'a pas été trouvée.");

                CategoriesDto categorieDto = new()
                {
                    CategoryId = id,
                    Name = categorie.Name
                };

                return Ok(categorieDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Erreur dans l'argument : " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound("Erreur lors de la récupération de la catégorie : " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur interne du serveur : " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoriesDto>> CreateCategorie([FromBody] NameCategorieDto model)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                Categories newCategorie = new()
                {
                    Name = model.Name,
                };

                await _service.AddCategorie(newCategorie);

                CategoriesDto categoriesDto = new()
                {
                    CategoryId = newCategorie.CategoryId,
                    Name = newCategorie.Name
                };

                return CreatedAtAction(nameof(GetCategorieById), new { id = categoriesDto.CategoryId}, categoriesDto);
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
        public async Task<ActionResult> DeleteCategories([FromRoute] int id)
        {
            try
            {
                var existingCategorie = await _service.GetCategorieById(id);

                if (existingCategorie == null) return NotFound($"La catégorie avec l'id {id} n'a pas été trouvée.");

                await _service.DeleteCategorie(id);

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


        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategories(int id,[FromBody] NameCategorieDto categorieDto)
        {
            try
            {
                var existingCategorie = await _service.GetCategorieById(id);

                if (existingCategorie == null) return NotFound("La catégorie n'a pas été trouvée.");

                existingCategorie.Name = categorieDto.Name;

                await _service.UptadeCategorie(existingCategorie);

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

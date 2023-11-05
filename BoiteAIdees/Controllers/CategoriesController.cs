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

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriesDto>> GetCategorieById(int id)
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

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCategories(int id, Categories categories)
        //{

        //}

        //[HttpPost]
        //public async Task<ActionResult<Categories>> PostCategories(Categories categories)
        //{
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCategories(int id)
        //{
        //}

        //private bool CategoriesExists(int id)
        //{
        //    return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        //}
    }
}

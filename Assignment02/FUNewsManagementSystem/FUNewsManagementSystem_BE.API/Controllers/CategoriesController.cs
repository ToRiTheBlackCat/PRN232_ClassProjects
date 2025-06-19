using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Service;
using FUNewsManagementSystem_BE.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        [Authorize(Roles = "2")]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                if (categories == null || !categories.Any())
                {
                    return NotFound("No categories found.");
                }
                return Ok(categories);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving categories: {ex.Message}");
            }
        }

        [HttpGet("{id}")] //DONE
        public async Task<ActionResult<Category>> GetCategoriesById(short id)
        {
            var cate = await _categoryService.GetCategoryByIdAsync(id);

            if (cate == null)
            {
                return NotFound();
            }

            return Ok(cate);
        }

        [HttpPost] //DONE
        public async Task<IActionResult> PostCategory(Category category)
        {
            await _categoryService.Create(category);

            return CreatedAtAction("GetCategories", new { id = category.CategoryId }, category);

        }
        [HttpPut("{id}")] //DONE
        public async Task<IActionResult> PutCategory(short id, Category category)
        {

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            try
            {
                var result = await _categoryService.Update(category);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
            return NoContent();
        }

        [HttpDelete("{id}")] //DONE
        public async Task<IActionResult> DeleteCategory(short id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var result = await _categoryService.Remove(category);
            if (result == true)
            {
                return Ok("Delete successfully");
            }
            else
            {
                return BadRequest("Category cannot be removed as it has associated news articles.");
            }
        }

    }
}
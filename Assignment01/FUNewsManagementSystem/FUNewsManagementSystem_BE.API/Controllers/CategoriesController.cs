using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Service;
using FUNewsManagementSystem_FE.MVCWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateC([FromBody] CategoryModel model)
        {
            try
            {
                var category = new Category
                {
                    CategoryId = model.CategoryId,
                    CategoryName = model.CategoryName,
                    CategoryDesciption = model.CategoryDesciption,
                    IsActive = model.IsActive
                };

                var result = await _categoryService.Create(category);
                if (result > 0)
                    return Ok("Category created successfully.");
                else
                    return BadRequest("Failed to create category.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryModel>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                if (categories == null || !categories.Any())
                {
                    return NotFound("No categories found.");
                }
                var convert = categories.Select(c => new CategoryModel()
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDesciption = c.CategoryDesciption,
                    IsActive = c.IsActive
                });
                return convert.ToList();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving categories: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCategory(short id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id); // Await added here
                if (category != null)
                {
                    var result = await _categoryService.Remove(category);
                    if (result == true)
                    {
                        return Ok("Category removed successfully.");
                    }
                    else
                    {
                        return BadRequest("Category cannot be removed as it has associated news articles.");
                    }
                }
                else
                {
                    return NotFound("Category not found."); // Added return for when category is null
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error removing category: {ex.Message}");
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryModel model)
        {
            try
            {
                var existingCategory = await _categoryService.GetCategoryByIdAsync(model.CategoryId);
                var category = new Category
                {
                    CategoryId = existingCategory.CategoryId,
                    CategoryName = existingCategory.CategoryName,
                    CategoryDesciption = existingCategory.CategoryDesciption,
                    IsActive = existingCategory.IsActive
                };
                var result = await _categoryService.Update(category);
                if (result > 0)
                    return Ok("Category updated successfully.");
                else
                    return BadRequest("Failed to update category.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}

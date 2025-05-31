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
        public async Task<ActionResult<CategoryModel>> CreateCategory(Category category)
        {
            try
            {
                var newCategory = await _categoryService.Create(category);
                if (newCategory > 0)
                {
                    return Ok("Category created successfully.");
                }
                else
                {
                    return BadRequest("Failed to create category.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating category: {ex.Message}");
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
                    var result = await _categoryService.RemoveCategoryAsync(category);
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
    }
}

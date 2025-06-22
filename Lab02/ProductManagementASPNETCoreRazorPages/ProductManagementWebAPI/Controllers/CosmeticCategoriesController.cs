using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("1")]
    public class CosmeticCategoriesController : ODataController
    {
        private readonly ICosmeticCategoryService _categoryService;

        public CosmeticCategoriesController(ICosmeticCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CosmeticCategory>>> Get()
        {
            return await _categoryService.Get();
        }


        [HttpGet("{key}")]
        public async Task<ActionResult<CosmeticCategory>> GetAsync([FromRoute] string key)
        {
            var cosmeticCategory = await _categoryService.Get(key);

            if (cosmeticCategory == null)
            {
                return NotFound();
            }

            return cosmeticCategory;
        }

        //// PUT: api/CosmeticCategories/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CosmeticCategory cosmeticCategory)
        {
            try
            {
                var result = await _categoryService.Update(cosmeticCategory);
                return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CosmeticCategoryExists(cosmeticCategory.CategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        // POST: api/CosmeticCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CosmeticCategory>> CreateCategory([FromBody] CosmeticCategory cosmeticCategory)
        {
            try
            {
                var result = await _categoryService.Add(cosmeticCategory);
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                if (CosmeticCategoryExists(cosmeticCategory.CategoryId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        // DELETE: api/CosmeticCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (!CosmeticCategoryExists(id))
            {
                return NotFound();
            }

            try
            {
                var result = await _categoryService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        private bool CosmeticCategoryExists(string id)
        {
            var item = _categoryService.Get(id).Result;
            return item != null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObjects;
using Services;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICatergoryService _catergory;

        public CategoriesController(ICatergoryService catergory)
        { 
            _catergory = catergory;
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            return _catergory.GetCategories();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _catergory.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            try
            {
                _catergory.UpdateCategory(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Category> PostCategory(Category category)
        {
            _catergory.SaveCategory(category);

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _catergory.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            _catergory.DeleteCategory(category);

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            var foundCategory = _catergory.GetCategoryById(id);
            return foundCategory != null ? true : false;
        }
    }
}

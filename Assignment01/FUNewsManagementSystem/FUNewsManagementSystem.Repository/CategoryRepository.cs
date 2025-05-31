using FUNewsManagementSystem.Repository.Base;
using FUNewsManagementSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Repository
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository()
        {

        }

        public async Task<bool> RemoveWithCheckingAsync(Category category)
        {
            if (category.NewsArticles == null)
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<Category> GetCategoryByIdAsync(short id)
        {
            var category = await _context.Categories
                .Include(c => c.InverseParentCategory)
                .Include(c => c.NewsArticles)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            return category;
        }

        public async Task<List<Category>> GetCategories()
        {
            var categories = await _context.Categories 
                .Include(c => c.InverseParentCategory)
                .Include(c => c.NewsArticles)
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
            return categories;
        }
    }
}

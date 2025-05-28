using FUNewsManagementSystem.Repository.Base;
using FUNewsManagementSystem.Repository.Models;
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
    }
}

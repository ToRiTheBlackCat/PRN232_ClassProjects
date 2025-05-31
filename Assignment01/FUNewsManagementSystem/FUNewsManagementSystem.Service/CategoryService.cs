using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Service
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        public CategoryService() => _categoryRepository = new CategoryRepository();
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategories();
        }

        public async Task<bool> RemoveCategoryAsync(Category category)
        {
            return await _categoryRepository.RemoveWithCheckingAsync(category);
        }
    }
}

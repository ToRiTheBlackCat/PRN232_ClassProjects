﻿using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Service
{
    public interface ICategoryService
    {
        Task<Category?> GetCategoryByIdAsync(short id);
        Task<List<Category>> GetCategoriesAsync();
        Task<int> Create(Category category);
        Task<bool> Remove(Category category);
        Task<int> Update(Category category);
    }

    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        public CategoryService() => _categoryRepository = new CategoryRepository();

        public async Task<Category?> GetCategoryByIdAsync(short id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }
        public async Task<int> Create(Category category)
        {
            return await _categoryRepository.CreateAsync(category);
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategories();
        }

        public async Task<bool> Remove(Category category)
        {
            return await _categoryRepository.RemoveWithCheckingAsync(category);
        }

        public async Task<int> Update(Category category)
        {
            return await _categoryRepository.UpdateAsync(category);
        }
    }
}

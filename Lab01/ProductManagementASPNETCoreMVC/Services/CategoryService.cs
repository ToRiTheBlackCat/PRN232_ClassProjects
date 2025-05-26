using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICatergoryService
    {
        private readonly ICatergoryRepository iCategoryRepository;
        public CategoryService()
        {
            iCategoryRepository = new CategoryRepository();
        }

        public void DeleteCategory(Category cate)
        {
            iCategoryRepository.DeleteCategory(cate);
        }

        public List<Category> GetCategories()
        {
            return iCategoryRepository.GetCategories();
        }

        public Category? GetCategoryById(int id)
        {
            return iCategoryRepository.GetCategoryById(id);
        }

        public void SaveCategory(Category cate)
        {
            iCategoryRepository.SaveCategory(cate);
        }

        public void UpdateCategory(Category cate)
        {
            iCategoryRepository.UpdateCategory(cate);
        }
    }
}

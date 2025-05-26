using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICatergoryRepository
    {
        public void CreateCategory(Category cate) => CategoryDAO.CreateCategory(cate);

        public void DeleteCategory(Category cate) => CategoryDAO.DeleteCategory(cate);
        public List<Category> GetCategories() => CategoryDAO.GetCategories();

        public Category? GetCategoryById(int id) => CategoryDAO.GetCategoryById(id);

        public void SaveCategory(Category cate) => CategoryDAO.SaveCategory(cate);

        public void UpdateCategory(Category cate) => CategoryDAO.UpdateCategory(cate);
    }
}

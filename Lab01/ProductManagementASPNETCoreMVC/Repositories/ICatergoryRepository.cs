using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICatergoryRepository
    {
        void SaveCategory(Category cate);
        void DeleteCategory(Category cate);
        void UpdateCategory(Category cate);
        void CreateCategory(Category cate);

        Category? GetCategoryById(int id);
        List<Category> GetCategories();

    }
}

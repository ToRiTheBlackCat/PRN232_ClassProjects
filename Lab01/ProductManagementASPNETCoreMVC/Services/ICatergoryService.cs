using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICatergoryService
    {
        void SaveCategory(Category cate);
        void DeleteCategory(Category cate);
        void UpdateCategory(Category cate);
        List<Category> GetCategories();
        Category? GetCategoryById(int id);
    }
}

using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class CategoryDAO
    {
        public static List<Category> GetCategories()
        {
            var listCategories = new List<Category>();
            try
            {
                using var context = new MyStoreDBContext();
                listCategories = context.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listCategories;
        }
        public static void SaveCategory(Category cate)
        {
            try
            {
                using var context = new MyStoreDBContext();
                context.Categories.Add(cate);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void CreateCategory(Category cate)
        {
            try
            {
                using var context = new MyStoreDBContext();
                context.Categories.Add(cate);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateCategory(Category cate)
        {
            try
            {
                using var context = new MyStoreDBContext();
                context.Entry<Category>(cate).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteCategory(Category cate)
        {
            try
            {
                using var context = new MyStoreDBContext();
                var foundCate =
                    context.Categories.SingleOrDefault(c => c.CategoryId == cate.CategoryId);

                if (foundCate != null)
                    context.Categories.Remove(foundCate);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Category? GetCategoryById(int id)
        {
            using var db = new MyStoreDBContext();
            return db.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

    }
}

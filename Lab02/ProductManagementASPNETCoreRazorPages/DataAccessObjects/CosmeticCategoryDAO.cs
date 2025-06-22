using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class CosmeticCategoryDAO
    {
        private static CosmeticCategoryDAO? instance = null;

        private CosmeticCategoryDAO() { }

        public static CosmeticCategoryDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CosmeticCategoryDAO();
                }
                return instance;
            }
        }

        public async Task<List<CosmeticCategory>> GetAll()
        {
            using (var context = new CosmeticsDBContext())
            {
                var listCategories = await context.CosmeticCategories.ToListAsync();
                return listCategories;
            }
        }

        public async Task<CosmeticCategory> GetById(string id)
        {
            using (var context = new CosmeticsDBContext())
            {
                var listCategories = await context.CosmeticCategories.FirstOrDefaultAsync(x => x.CategoryId.Equals(id));
                return listCategories;
            }
        }

        public async Task<CosmeticCategory> Add(CosmeticCategory category)
        {
            using (var context = new CosmeticsDBContext())
            {
                category.CategoryId = GenerateId();
                await context.CosmeticCategories.AddAsync(category);
                await context.SaveChangesAsync();
                return category;
            }
        }

        public async Task<CosmeticCategory> Update(CosmeticCategory category)
        {
            using (var context = new CosmeticsDBContext())
            {
                context.CosmeticCategories.Update(category);
                await context.SaveChangesAsync();
                return category;
            }
        }

        public async Task<CosmeticCategory> Delete(string id)
        {
            using (var context = new CosmeticsDBContext())
            {
                var category = await context.CosmeticCategories.FirstOrDefaultAsync(x => x.CategoryId.Equals(id));
                if (category == null)
                {
                    throw new Exception("Category not found.");
                }

                context.CosmeticCategories.Remove(category);
                await context.SaveChangesAsync();
                return category;
            }
        }

        private string GenerateId()
        {
            var id = "CAT";
            using (var context = new CosmeticsDBContext())
            {
                var category = context.CosmeticCategories.ToList();
                if (category == null)
                {
                    return id + "0";
                }

                var maxId = category.OrderByDescending(x => x.CategoryId).First();
                id = id + (int.Parse(maxId.CategoryId.Substring(3)) + 1).ToString("D7");
            }
            return id;
        }
    }
}

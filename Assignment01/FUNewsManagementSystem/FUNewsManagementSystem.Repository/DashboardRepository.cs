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
    public class DashboardRepository : GenericRepository<NewsArticleModel>
    {
        public DashboardRepository()
        {
        }

        /*
        private List<short> GetAllDescendantCategoryIds(short parentId, List<Category> allCategories)
        {
            var descendants = new List<short>();

            //go through the provided list, get all category that match the provided category's id
            var children = allCategories
                .Where(c => c.ParentCategoryId == parentId)
                .ToList();

            //base case is when children is null (meaning no child is found for the given category)
            //foreach wont run, and it would return null to the previous call's AddRange() which adds nothing

            foreach (var child in children)
            {
                descendants.Add(child.CategoryId);
                //recursive go through the list again, now with each child of the initial category
                descendants.AddRange(GetAllDescendantCategoryIds(child.CategoryId, allCategories));
            }

            return descendants;
        }

        public async Task<int> GetTotalNewsCount(string categoryName)
        {
            int count = 0;
            Category? category = await _context.Categories
                .Include(a => a.InverseParentCategory)
                .FirstOrDefaultAsync(x => x.CategoryName == categoryName);

            if (string.IsNullOrEmpty(categoryName) || category == null)
            {
                count = await _context.NewsArticles.CountAsync();
                return count;
            }

            if ((category.ParentCategoryId == null || category.CategoryId == category.ParentCategoryId) || true) //topmost category or otherwise
            {
                //load all categories once for recursive traversal
                var allCategories = await _context.Categories.ToListAsync();

                //get list of all category IDs to include
                var categoryIds = new List<short> { category.CategoryId };

                //start the recursion call to get all children of providied category
                var allDescendants = GetAllDescendantCategoryIds(category.CategoryId, allCategories);
                //transfer the children id list from the recursive function to this function's list
                categoryIds.AddRange(allDescendants);
                count = await _context.NewsArticles
                    .CountAsync(n => categoryIds.Contains((short) n.CategoryId));
            }
            return count;
        }
        */

        public async Task<int> GetTotalNewsCount(string categoryName)
        {
            int count = 0;
            Category? category = await _context.Categories
            .Include(a => a.InverseParentCategory)
            .FirstOrDefaultAsync(x => x.CategoryName.Contains(categoryName));

            if (category == null)
            {
                count = await _context.NewsArticles.CountAsync();
            }
            else if (category.ParentCategoryId == null || category.CategoryId == category.ParentCategoryId || true) //topmost category or any category (can only get immediate child)
            {
                //Get all immediate child of given category
                List<Category> childCate = await _context.Categories.Where(x => x.ParentCategoryId == category.CategoryId).ToListAsync();
                foreach (var cate in childCate)
                {
                    count += await _context.NewsArticles.Where(x => x.CategoryId == cate.CategoryId).CountAsync();
                }
            }
            return count;
        }

        public async Task<int> GetTotalNewsCountByDate(DateTime fromDate, DateTime toDate)
        {
            return _context.NewsArticles
                .Where(x => x.CreatedDate >= fromDate && x.CreatedDate < toDate.AddDays(1))
                .Count();
        }
    }
}
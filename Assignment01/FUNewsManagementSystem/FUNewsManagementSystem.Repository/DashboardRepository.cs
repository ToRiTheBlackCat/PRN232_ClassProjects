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
    public class DashboardRepository : GenericRepository<NewsArticle>
    {
        public DashboardRepository()
        {

        }

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

            //load all categories once for recursive traversal
            var allCategories = await _context.Categories.ToListAsync();

            //get list of all category IDs to include
            var categoryIds = new List<short> { category.CategoryId };

            if (string.IsNullOrEmpty(categoryName) || category == null)
            {
                count = await _context.NewsArticles.CountAsync();
            }

            if ((category.ParentCategoryId == null || category.CategoryId == category.ParentCategoryId) || true) //topmost category or otherwise
            {
                //start the recursion call to get all children of providied category
                var allDescendants = GetAllDescendantCategoryIds(category.CategoryId, allCategories);
                //transfer the children id list from the recursive function to this function's list
                categoryIds.AddRange(allDescendants);
                count = await _context.NewsArticles
                    .CountAsync(n => categoryIds.Contains((short) n.CategoryId));

            }
            return count;
        }
    }
}

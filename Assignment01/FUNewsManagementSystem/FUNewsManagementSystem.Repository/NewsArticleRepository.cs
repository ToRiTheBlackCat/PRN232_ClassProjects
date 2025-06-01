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
    public class NewsArticleRepository : GenericRepository<NewsArticle>
    {
        public NewsArticleRepository()
        {

        }

        public new async Task<List<NewsArticle>?> GetAll()
        {
            var itemList = await _context.NewsArticles
                .Include(x => x.Tags)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return itemList;
        }

        public async Task<List<NewsArticle>?> GetAllByDate(DateTime fromDate, DateTime toDate)
        { 
            var itemList = await _context.NewsArticles
                .Include(x => x.Tags)
                .Where(x => x.CreatedDate >= fromDate && x.CreatedDate < toDate.AddDays(1))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return itemList;
        }

        public new async Task<NewsArticle?> GetById(string id)
        {
            var itemList = await _context.NewsArticles
                .FirstOrDefaultAsync(x => x.NewsArticleId.Equals(id));

            return itemList;
        }

        public async Task<List<NewsArticle>> Search(string? newsTitle, string? headline, string? newsContent, string? categoryName)
        {
            var itemList = await _context.NewsArticles
                .Include(x => x.Tags)
                .Include(x => x.Category)
                .Where(x =>
                    string.IsNullOrEmpty(newsTitle) || x.NewsTitle.Contains(newsTitle) ||
                    string.IsNullOrEmpty(headline) || x.Headline.Contains(headline) ||
                    string.IsNullOrEmpty(newsContent) || x.NewsContent.Contains(newsContent) ||
                    string.IsNullOrEmpty(categoryName) || x.Category.CategoryName.Contains(categoryName))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return itemList;
        }

    }
}

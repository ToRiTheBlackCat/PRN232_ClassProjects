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
    public class NewsArticleRepository : GenericRepository<NewsArticleModel>
    {
        public NewsArticleRepository()
        {

        }

        public new async Task<List<NewsArticleModel>?> GetAll()
        {
            var itemList = await _context.NewsArticles
                .Include(x => x.Tags)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return itemList;
        }

        public new async Task<NewsArticleModel?> GetById(string id)
        {
            var itemList = await _context.NewsArticles
                .Include(x => x.Tags) 
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.NewsArticleId.Equals(id));

            return itemList;
        }

        public async Task<List<NewsArticleModel>> Search(string? newsTitle, string? headline, string? newsContent, string? categoryName)
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

        public async Task<List<NewsArticleModel>> SearchTitle(string? newTitle)
        {
            var itemList = await _context.NewsArticles
                .Include(x => x.Tags)
                .Include(x => x.Category)
                .Where(x => string.IsNullOrEmpty(newTitle) || x.NewsTitle.Contains(newTitle))
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
            return itemList;

        }
    }
}

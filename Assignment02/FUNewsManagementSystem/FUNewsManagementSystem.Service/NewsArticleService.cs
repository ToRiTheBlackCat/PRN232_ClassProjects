using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Repository.Models.FormModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Service
{
    public class NewsArticleService
    {
        private readonly NewsArticleRepository _newsArticleRepository;
        public NewsArticleService()
        {
            _newsArticleRepository = new NewsArticleRepository();
        }

        public async Task<List<NewsArticleModel>?> GetAllAsync()
        {
            return await _newsArticleRepository.GetAll();
        }

        public async Task<List<NewsArticleModel>?> SearchAsync(string? newsTitle, string? headline, string? newsContent, string? categoryName)
        {
            return await _newsArticleRepository.SearchTitle(newsTitle/*, headline, newsContent, categoryName*/);
        }

        public async Task<NewsArticleModel?> GetByIdAsync(string id)
        {
            return await _newsArticleRepository.GetById(id);
        }

        public new async Task<List<NewsArticleModel>?> GetAll()
        {
            return await _newsArticleRepository.GetAll();
        }

        public async Task<bool> AddAsync(NewsArticleModel newsArticle)
        {
            List<NewsArticleModel> existingArticles = await _newsArticleRepository.GetAll();
            if (newsArticle == null)
            {
                throw new ArgumentNullException(nameof(newsArticle), "News article cannot be null.");
            }
            newsArticle.NewsArticleId = (existingArticles.Count + 1).ToString();
            newsArticle.CreatedDate = DateTime.UtcNow;
            newsArticle.ModifiedDate = DateTime.UtcNow;
            newsArticle.NewsStatus = true;
            newsArticle.CategoryId = 1;
            newsArticle.CreatedById = 1; 
            newsArticle.UpdatedById = 1; // Assuming default user ID is 1
            await _newsArticleRepository.CreateAsync(newsArticle);
            return true;
            
        }

        public async Task<List<NewsArticleModel>?> GetAllByDate(DateTime fromDate, DateTime toDate)
        {
            
            return await _newsArticleRepository.GetAllByDate(fromDate, toDate);
        }

        public async Task<bool> UpdateAsync(NewsArticleModel newsArticle)
        {
            if (newsArticle == null)
            {
                throw new ArgumentNullException(nameof(newsArticle), "News article cannot be null.");
            }
            var existingArticle = await _newsArticleRepository.GetById(newsArticle.NewsArticleId);
            newsArticle.CreatedDate = existingArticle?.CreatedDate;
            newsArticle.ModifiedDate = DateTime.UtcNow; // Update the modified date
            newsArticle.CategoryId = existingArticle?.CategoryId; // Preserve the category ID
            newsArticle.CreatedById = existingArticle?.CreatedById;
            newsArticle.NewsStatus = existingArticle?.NewsStatus ?? true; // Preserve the news status
            if (newsArticle.UpdatedById == null)
            {
                newsArticle.UpdatedById = existingArticle?.UpdatedById; // Preserve the updated by ID if not set
            }
            await _newsArticleRepository.UpdateAsync(newsArticle);
            return true;
        }

        public new async Task<NewsArticleModel?> GetById(string id)
        {
            return await _newsArticleRepository.GetById(id);
        }

        public async Task<bool> DeleteAsync(NewsArticleModel newsArticle)
        {
            if (newsArticle == null)
            {
                throw new ArgumentNullException(nameof(newsArticle), "News article cannot be null.");
            }
            return await _newsArticleRepository.RemoveAsync(newsArticle);
        }

        public async Task<List<NewsArticleModel>> Search(string? newsTitle, string? headline, string? newsContent, string? categoryName)
        {
            return await _newsArticleRepository.Search(newsTitle, headline, newsContent, categoryName);
        }

        public async Task<List<NewsArticleModel>> GetTop5NewestByCategory(string? categoryName)
        {
            return await _newsArticleRepository.GetTop5NewestByCategory(categoryName);
        }
    }
}
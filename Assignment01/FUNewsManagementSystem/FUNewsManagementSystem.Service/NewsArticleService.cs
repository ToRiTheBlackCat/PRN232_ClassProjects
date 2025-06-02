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
        public NewsArticleService(NewsArticleRepository repo)
        {
            return await _newsArticleRepository.SearchTitle(newsTitle/*, headline, newsContent, categoryName*/);
            _repo = repo;
        }

        public async Task<NewsArticleModel?> GetByIdAsync(string id)
        public new async Task<List<NewsArticle>?> GetAll()
        {
            return await _newsArticleRepository.GetById(id);
            return await _repo.GetAll();
        }

        public async Task<bool> AddAsync(NewsArticleModel newsArticle)
        public async Task<List<NewsArticle>?> GetAllByDate(DateTime fromDate, DateTime toDate)
        {
            List<NewsArticleModel> existingArticles = await _newsArticleRepository.GetAll();
            if (newsArticle == null)
            {
                throw new ArgumentNullException(nameof(newsArticle), "News article cannot be null.");
            }
            newsArticle.NewsArticleId = new string((existingArticles.Count+1).ToString()); 
            newsArticle.CreatedDate = DateTime.UtcNow; 
            newsArticle.ModifiedDate = DateTime.UtcNow; 
            newsArticle.NewsStatus = true; 
            newsArticle.CategoryId = 1; 
            await _newsArticleRepository.CreateAsync(newsArticle);
            return true;
            return await _repo.GetAllByDate(fromDate, toDate);
        }

        public async Task<bool> UpdateAsync(NewsArticleModel newsArticle)
        {
            if (newsArticle == null)
        public new async Task<NewsArticle?> GetById(string id)
        {
                throw new ArgumentNullException(nameof(newsArticle), "News article cannot be null.");
            }
            var existingArticle = await _newsArticleRepository.GetById(newsArticle.NewsArticleId);
            newsArticle.ModifiedDate = DateTime.UtcNow; // Update the modified date
            newsArticle.CategoryId = existingArticle?.CategoryId; // Preserve the category ID
            await _newsArticleRepository.UpdateAsync(newsArticle);
            return true;
            return await _repo.GetById(id);
        }

        public async Task<bool> DeleteAsync(NewsArticleModel newsArticle)
        {
            if (newsArticle == null)
        public async Task<List<NewsArticle>> Search(string? newsTitle, string? headline, string? newsContent, string? categoryName)
        {
                throw new ArgumentNullException(nameof(newsArticle), "News article cannot be null.");
            return await _repo.Search(newsTitle, headline, newsContent, categoryName);
        }
            return await _newsArticleRepository.RemoveAsync(newsArticle);
        public async Task<List<NewsArticle>> GetTop5NewestByCategory(string? categoryName)
        {
            return await _repo.GetTop5NewestByCategory(categoryName);
        }
    }
}

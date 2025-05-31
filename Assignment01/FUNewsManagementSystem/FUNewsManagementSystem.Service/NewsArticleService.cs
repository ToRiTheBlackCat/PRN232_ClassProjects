using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Repository.Models;
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
        public async Task<List<NewsArticle>?> GetAllAsync()
        {
            return await _newsArticleRepository.GetAll();
        }

        public async Task<List<NewsArticle>?> SearchAsync(string? newsTitle, string? headline, string? newsContent, string? categoryName)
        {
            return await _newsArticleRepository.Search(newsTitle, headline, newsContent, categoryName);
        }

        public async Task<NewsArticle?> GetByIdAsync(string id)
        {
            return await _newsArticleRepository.GetById(id);
        }

        public async Task<bool> AddAsync(NewsArticle newsArticle)
        {
            if (newsArticle == null)
            {
                throw new ArgumentNullException(nameof(newsArticle), "News article cannot be null.");
            }
            await _newsArticleRepository.CreateAsync(newsArticle);
            return true;
        }

        public async Task<bool> UpdateAsync(NewsArticle newsArticle)
        {
            if (newsArticle == null)
            {
                throw new ArgumentNullException(nameof(newsArticle), "News article cannot be null.");
            }
            await _newsArticleRepository.UpdateAsync(newsArticle);
            return true;
        }

        public async Task<bool> DeleteAsync(NewsArticle newsArticle)
        {
            if (newsArticle == null)
            {
                throw new ArgumentNullException(nameof(newsArticle), "News article cannot be null.");
            }
            return await _newsArticleRepository.RemoveAsync(newsArticle);
        }
    }
}

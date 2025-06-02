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
        private readonly NewsArticleRepository _repo;

        public NewsArticleService(NewsArticleRepository repo)
        {
            _repo = repo;
        }

        public new async Task<List<NewsArticle>?> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<List<NewsArticle>?> GetAllByDate(DateTime fromDate, DateTime toDate)
        {
            return await _repo.GetAllByDate(fromDate, toDate);
        }

        public new async Task<NewsArticle?> GetById(string id)
        {
            return await _repo.GetById(id);
        }

        public async Task<List<NewsArticle>> Search(string? newsTitle, string? headline, string? newsContent, string? categoryName)
        {
            return await _repo.Search(newsTitle, headline, newsContent, categoryName);
        }
        public async Task<List<NewsArticle>> GetTop5NewestByCategory(string? categoryName)
        {
            return await _repo.GetTop5NewestByCategory(categoryName);
        }
    }
}

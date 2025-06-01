using AutoMapper;
using FUNewsManagementSystem.Repository.Models.FormModels;
using FUNewsManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly DashboardService _servDashboard;
        private readonly NewsArticleService _servNews;
        private readonly IMapper _mapper;

        public DashboardController(DashboardService servDashboard, NewsArticleService servNews, IMapper mapper)
        {
            _servDashboard = servDashboard;
            _servNews = servNews;
            _mapper = mapper;
        }

        [HttpGet("/newsCount/category")]
        public async Task<IActionResult> GetTotalNewsCountWithCategory(string? categoryName)
        {
            int result = 0;
            if (categoryName == null) result = await _servDashboard.GetTotalNewsCount();
            else result = await _servDashboard.GetTotalNewsCountByCategory(categoryName);
            return Ok(result);
        }

        [HttpGet("/newsCount/date")]
        public async Task<IActionResult> GetNewsCountWithinDate(DateTime fromDate, DateTime toDate)
        {
            int result = await _servDashboard.GetTotalNewsCountByDate(fromDate, toDate);
            return Ok(result);
        }
        
        [HttpGet("/news/date")]
        public async Task<IActionResult> GetNewsWithinDate(DateTime fromDate, DateTime toDate)
        {
            var list = await _servNews.GetAllByDate(fromDate, toDate);
            return list == null
                ? NotFound(new
                {
                    Message = "No news found within date"
                })
                : Ok(_mapper.Map<List<NewsArticleView>>(list));
        }
    }
}

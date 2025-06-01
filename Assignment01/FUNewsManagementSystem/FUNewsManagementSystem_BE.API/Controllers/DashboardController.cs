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

        public DashboardController(DashboardService servDash, NewsArticleService servNews)
        {
            _servDashboard = servDash;
            _servNews = servNews;
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalNewsCountWithCategory(string? categoryName)
        {
            int result = 0;
            if (categoryName == null) result = await _servDashboard.GetTotalNewsCount();
            else result = await _servDashboard.GetTotalNewsCountByCategory(categoryName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetNewsCountWithinDate(DateTime fromDate, DateTime toDate)
        {
            int result = await _servDashboard.GetTotalNewsCountByDate(fromDate, toDate);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetNewsWithinDate(DateTime fromDate, DateTime toDate)
        {
            var list = _servNews.GetAllByDate(fromDate, toDate);
            return list == null
                ? NotFound(new
                {
                    Message = "No news found within date"
                })
                : Ok(list);
        }
    }
}

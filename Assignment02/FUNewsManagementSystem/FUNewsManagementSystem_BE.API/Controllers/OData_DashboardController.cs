using AutoMapper;
using AutoMapper.QueryableExtensions;
using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Repository.Models.FormModels;
using FUNewsManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    [Route("odata/dashboard")]
    [ApiController]
    public class OData_DashboardController : ControllerBase
    {
        private readonly DashboardService _servDashboard;
        private readonly NewsArticleService _servNews;
        private readonly IMapper _mapper;

        public OData_DashboardController(DashboardService servDashboard, NewsArticleService servNews, IMapper mapper)
        {
            _servDashboard = servDashboard;
            _servNews = servNews;
            _mapper = mapper;
        }

        [HttpGet("newsCount")]
        //[EnableQuery]
        public async Task<IActionResult> GetTotalNewsCountOData([FromODataUri]string? categoryName, [FromODataUri] DateTime? fromDate, [FromODataUri] DateTime? toDate)
        {
            int result = 0;

            if (fromDate.HasValue && toDate.HasValue)
            {
                result = await _servDashboard.GetTotalNewsCountByDate(fromDate.Value, toDate.Value);
            }
            else if (!string.IsNullOrEmpty(categoryName))
            {
                result = await _servDashboard.GetTotalNewsCountByCategory(categoryName);
            }
            else
            {
                result = await _servDashboard.GetTotalNewsCount();
            }

            return Ok(result);
        }

        [EnableQuery(PageSize = 10)]
        [HttpGet("news")]
        public async Task<ActionResult<IQueryable<NewsArticleView>>> GetNews()
        {
            var articles = await _servNews.GetAllAsync();
            var queryable = articles.AsQueryable().ProjectTo<NewsArticleView>(_mapper.ConfigurationProvider);
            return Ok(queryable);
        }

    }
}

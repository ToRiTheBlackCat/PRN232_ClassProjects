using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Results;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    [Route("odata/[controller]")]
    public class OData_NewsArticlesController : ODataController
    {
        private readonly NewsArticleService _newsArticleService;

        public OData_NewsArticlesController(NewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        /// <summary>
        /// GET: odata/OData_NewsArticles
        /// </summary>
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var articles = await _newsArticleService.GetAllAsync();
            return Ok(articles.AsQueryable());
        }

        /// <summary>
        /// GET: odata/OData_NewsArticles('1')
        /// </summary>
        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromODataUri] string key)
        {
            var article = await _newsArticleService.GetByIdAsync(key);
            if (article == null)
                return NotFound();

            return Ok(SingleResult.Create(new[] { article }.AsQueryable()));
        }
    }
}

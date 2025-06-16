using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Results;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Route("odata/NewsArticles")]
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
        [EnableQuery(PageSize = 10)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsArticleModel>>> Get()
        {
            var articles = await _newsArticleService.GetAllAsync();
            return Ok(articles);
        }

        /// <summary>
        /// GET: odata/OData_NewsArticles('1')
        /// </summary>
        [EnableQuery]
        [HttpGet("{id}")]
        public async Task<SingleResult<NewsArticleModel>> Get([FromODataUri] string key)
        {
            var article = await _newsArticleService.GetByIdAsync(key);
            return SingleResult.Create(new[] { article }.AsQueryable());
        }

    }
}

using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Results;
using FUNewsManagementSystem.Repository.Models.FormModels;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    //[Route("odata/NewsArticles")]
    public class NewsArticlesController : ODataController
    {
        private readonly NewsArticleService _newsArticleService;

        public NewsArticlesController(NewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        /// <summary>
        /// GET: odata/OData_NewsArticles
        /// </summary>
        [EnableQuery(PageSize = 10)]
        public async Task<ActionResult<IEnumerable<NewsArticleModel>>> Get()
        {
            var articles = await _newsArticleService.GetAllAsync();
            return Ok(articles);
        }

        /// <summary>
        /// GET: odata/OData_NewsArticles('1')
        /// </summary>
        [EnableQuery]
        public async Task<SingleResult<NewsArticleModel>> Get([FromODataUri] string key)
        {
            var article = await _newsArticleService.GetByIdAsync(key);
            return SingleResult.Create(new[] { article }.AsQueryable());
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] NewsArticleModel newsArticle)
        {
            if (newsArticle == null)
            {
                return BadRequest("News article cannot be null.");
            }           

            var result = await _newsArticleService.AddAsync(newsArticle);
            if (result)
            {
                return CreatedAtAction(nameof(Get), new { key = newsArticle.NewsArticleId }, newsArticle);
            }
            return BadRequest("Failed to add the news article.");

        }

        [EnableQuery]
        [HttpPut("odata/NewsArticles({id})")]
        public async Task<IActionResult> Put([FromODataUri] string id, [FromBody] NewsArticleModel newsArticle)
        {
            if (newsArticle == null)
            {
                return BadRequest("News article cannot be null.");
            }
            if (id != newsArticle.NewsArticleId)
            {
                return BadRequest("News article ID in URL does not match ID in body.");
            }
            var existingArticle = await _newsArticleService.GetByIdAsync(id);
            if (existingArticle == null)
            {
                return NotFound();
            }
            await _newsArticleService.UpdateAsync(newsArticle);
            return NoContent();
        }

        [EnableQuery]
        [HttpDelete("odata/NewsArticles({id})")]
        public async Task<IActionResult> Delete([FromODataUri] string id)
        {
            var existingArticle = await _newsArticleService.GetByIdAsync(id);
            if (existingArticle == null)
            {
                return NotFound();
            }
            // Assuming you have a method to delete the article
            await _newsArticleService.DeleteAsync(existingArticle);
            return NoContent(); // Return 204 No Content on successful deletion
        }
    }
}

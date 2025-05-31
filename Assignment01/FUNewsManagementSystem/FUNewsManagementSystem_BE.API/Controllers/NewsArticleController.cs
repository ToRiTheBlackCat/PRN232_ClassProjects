    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;

    namespace FUNewsManagementSystem_BE.API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class NewsArticleController : ControllerBase
        {
            private readonly FUNewsManagementSystem.Service.NewsArticleService _newsArticleService;
            public NewsArticleController()
            {
                _newsArticleService = new FUNewsManagementSystem.Service.NewsArticleService();
            }
            [HttpGet("GetAll")]
            public async Task<IActionResult> GetAll()
            {
                var articles = await _newsArticleService.GetAllAsync();
                return Ok(articles);
            }
            [HttpGet("Search")]
            public async Task<IActionResult> Search(string? newsTitle, string? headline, string? newsContent, string? categoryName)
            {
                var articles = await _newsArticleService.SearchAsync(newsTitle, headline, newsContent, categoryName);
                return Ok(articles);
            }
            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(string id)
            {
                var article = await _newsArticleService.GetByIdAsync(id);
                if (article == null)
                {
                    return NotFound();
                }
                return Ok(article);
            }
            [HttpPost]
            public async Task<IActionResult> Add([FromBody] FUNewsManagementSystem.Repository.Models.NewsArticleModel newsArticle)
            {
                if (newsArticle == null)
                {
                    return BadRequest("News article cannot be null.");
                }
                await _newsArticleService.AddAsync(newsArticle);
                return CreatedAtAction(nameof(GetById), new { id = newsArticle.NewsArticleId }, newsArticle);
            }
            [HttpPut]
            public async Task<IActionResult> Update([FromBody] FUNewsManagementSystem.Repository.Models.NewsArticleModel newsArticle)
            {
                if (newsArticle == null)
                {
                    return BadRequest("News article cannot be null.");
                }
                await _newsArticleService.UpdateAsync(newsArticle);
                return NoContent();
            }
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(string id)
            {
                var article = await _newsArticleService.GetByIdAsync(id);
                if (article == null)
                {
                    return NotFound();
                }
                await _newsArticleService.DeleteAsync(article);
                return NoContent();
            }
        }
    }

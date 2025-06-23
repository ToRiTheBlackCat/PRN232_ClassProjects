using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Repository.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.NewsArticles
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IHttpClientFactory clientFactory, ILogger<CreateModel> logger)
        {
            _httpClient = clientFactory.CreateClient();
            _logger = logger;
        }

        [BindProperty]
        public NewsArticleView Article { get; set; }

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            Article.NewsArticleId = "1";
            Article.CategoryId ??= 1;
            Article.CreatedById ??= 1;
            Article.UpdatedById ??= 1;
            Article.CreatedDate ??= DateTime.UtcNow;
            Article.ModifiedDate ??= DateTime.UtcNow;
            Article.NewsStatus ??= true;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // Use PascalCase
            };
            var newsArticle = new NewsArticleModel
            {
                NewsArticleId = Article.NewsArticleId,
                NewsTitle = Article.NewsTitle,
                Headline = Article.Headline,
                CreatedDate = Article.CreatedDate,
                NewsContent = Article.NewsContent,
                NewsSource = Article.NewsSource,
                CategoryId = Article.CategoryId,
                NewsStatus = Article.NewsStatus,
                CreatedById = Article.CreatedById,
                UpdatedById = Article.UpdatedById,
                ModifiedDate = Article.ModifiedDate
            };
            _logger.LogInformation("Sending JSON: {Json}", JsonSerializer.Serialize(newsArticle, options));
            var content = new StringContent(JsonSerializer.Serialize(newsArticle, options), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:55171/odata/NewsArticles", content);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");
            if (!response.IsSuccessStatusCode)
            {
                var conres = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to post article. Status: {Status}, Response: {Content}", response.StatusCode, conres);
            }
            ModelState.AddModelError("", "Create failed");
            return Page();
        }
    }
}

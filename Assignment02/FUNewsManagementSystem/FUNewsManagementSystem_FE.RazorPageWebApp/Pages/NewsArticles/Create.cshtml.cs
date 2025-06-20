using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Repository.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

            var response = await _httpClient.PostAsJsonAsync("http://localhost:55171/odata/NewsArticles", Article);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to post article. Status: {Status}, Response: {Content}", response.StatusCode, content);
            }
            ModelState.AddModelError("", "Create failed");
            return Page();
        }
    }
}

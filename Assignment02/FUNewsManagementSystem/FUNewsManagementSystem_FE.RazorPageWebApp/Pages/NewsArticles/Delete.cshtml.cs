using FUNewsManagementSystem.Repository.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.NewsArticles
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(IHttpClientFactory clientFactory, ILogger<DeleteModel> logger)
        {
            _httpClient = clientFactory.CreateClient();
            _logger = logger;
        }

        [BindProperty]
        public NewsArticleView Article { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Article = await _httpClient.GetFromJsonAsync<NewsArticleView>($"https://localhost:55171/odata/NewsArticles('{id}')");
            if (Article == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:55171/odata/NewsArticles('{Article.NewsArticleId}')");
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to post article. Status: {Status}, Response: {Content}", response.StatusCode, content);
            }

            ModelState.AddModelError("", "Delete failed");
            return Page();
        }
    }
}

using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Repository.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.NewsArticles
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
        }

        [BindProperty]
        public NewsArticleModel Article { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Article = await _httpClient.GetFromJsonAsync<NewsArticleModel>($"https://localhost:55171/odata/NewsArticles('{id}')");
            if (Article == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var response = await _httpClient.PutAsJsonAsync($"https://localhost:55171/odata/NewsArticles('{Article.NewsArticleId}')", Article);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError("", "Update failed");
            return Page();
        }
    }
}

using FUNewsManagementSystem.Repository.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.NewsArticles
{
    public class DetailModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailModel(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
        }

        public NewsArticleView Article { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<NewsArticleView>($"https://localhost:55171/odata/NewsArticles('{id}')");
            if (response == null)
                return NotFound();

            Article = response;
            return Page();
        }
    }
}

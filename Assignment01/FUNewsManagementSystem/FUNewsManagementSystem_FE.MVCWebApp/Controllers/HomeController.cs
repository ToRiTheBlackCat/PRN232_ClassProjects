using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem_FE.MVCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using NewsArticleModel = FUNewsManagementSystem_FE.MVCWebApp.Models.NewsArticleModel;

namespace FUNewsManagementSystem_FE.MVCWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> IndexAsync(string? searchQuery)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                string requestUri;

                if (string.IsNullOrEmpty(searchQuery))
                {
                    requestUri = "NewsArticle/GetAll"; // Fetch all articles
                }
                else
                {
                    requestUri = $"NewsArticle/Search?newsTitle={Uri.EscapeDataString(searchQuery)}"; // Search articles
                }
                var response = await client.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    var wrappedModel = await response.Content.ReadFromJsonAsync<ReferencePreservedList<NewsArticleModel>>();

                    if (wrappedModel?.Values == null) // Check if values exist
                    {
                        return View(new List<FUNewsManagementSystem_FE.MVCWebApp.Models.NewsArticleModel>());
                    }

                    return View(wrappedModel.Values);
                }

                return View(new List<FUNewsManagementSystem_FE.MVCWebApp.Models.NewsArticleModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching news articles");
                return View(new List<FUNewsManagementSystem_FE.MVCWebApp.Models.NewsArticleModel>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Detail(string id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync($"NewsArticle/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var article = await response.Content.ReadFromJsonAsync<NewsArticleModel>();
                    return View(article);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching news article details");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
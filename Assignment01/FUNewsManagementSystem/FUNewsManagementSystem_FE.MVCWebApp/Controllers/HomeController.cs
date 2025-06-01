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
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
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
                    var wrappedModel = await response.Content.ReadFromJsonAsync<List<NewsArticleModel>>();

                    if (wrappedModel == null) // Check if values exist
                    {
                        return View(new List<FUNewsManagementSystem_FE.MVCWebApp.Models.NewsArticleModel>());
                    }

                    return View(wrappedModel);
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
                ViewBag.ApiBaseUrl = _configuration.GetValue<string>("ApiBaseUrl");
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

        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against Cross-Site Request Forgery (CSRF)
        public async Task<IActionResult> CreateNewsArticle(NewsArticleModel newsArticle)
        {
            // Note: NewsArticleId, CreatedDate, ModifiedDate, CreatedById, UpdatedById, NewsStatus
            // are often set by the backend API or automatically by the database.
            // If the backend API requires these, you might need to populate some here.
            // For demonstration, let's assume the backend handles ID and dates,
            // but you might need to set a placeholder for CreatedById if your API validates it.

            //if (ModelState.IsValid)
            
                try
                {
                    var client = _httpClientFactory.CreateClient("ApiClient");
                    var response = await client.PostAsJsonAsync("NewsArticle/Add", newsArticle); 
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "News article created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError("API Error creating news article: {StatusCode} - {Content}", response.StatusCode, errorContent);
                        ModelState.AddModelError(string.Empty, $"Error creating news article: {response.ReasonPhrase}. Details: {errorContent}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during news article creation.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the news article.");
                }
            

            // If ModelState is not valid or API call failed, re-display the Index view
            // and attempt to re-open the modal with validation errors.
            // Re-fetch necessary data for the view:
            List<NewsArticleModel> articles = new List<NewsArticleModel>();
            List<CategoryModel> categories = new List<CategoryModel>();

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var articlesResponse = await client.GetAsync("NewsArticle/GetAll");
                if (articlesResponse.IsSuccessStatusCode)
                {
                    var wrappedModel = await articlesResponse.Content.ReadFromJsonAsync<List<NewsArticleModel>>();
                    if (wrappedModel != null)
                    {
                        articles = wrappedModel;
                    }
                }

                //var categoriesResponse = await client.GetAsync("Category/GetAll");
                //if (categoriesResponse.IsSuccessStatusCode)
                //{
                //    var wrappedCategories = await categoriesResponse.Content.ReadFromJsonAsync<ReferencePreservedList<CategoryModel>>();
                //    if (wrappedCategories?.Values != null)
                //    {
                //        categories = wrappedCategories.Values;
                //    }
                //    else
                //    {
                //        categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryModel>>();
                //    }
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error re-fetching data for Index view after CreateNewsArticle failure.");
            }

            //ViewBag.Categories = categories;
            return View("Index", articles); // Return to Index view with current articles and errors
        }
    }
}
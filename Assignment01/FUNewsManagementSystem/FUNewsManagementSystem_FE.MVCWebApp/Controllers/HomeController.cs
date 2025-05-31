using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem_FE.MVCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync("NewsArticle/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var wrappedModel = await response.Content.ReadFromJsonAsync<ReferencePreservedList<FUNewsManagementSystem.Repository.Models.NewsArticleModel>>();

                    if (wrappedModel?.Values == null) // Check if values exist
                    {
                        return View(new List<FUNewsManagementSystem_FE.MVCWebApp.Models.NewsArticleModel>());
                    }

                    var feModel = wrappedModel.Values.Select(b => new Models.NewsArticleModel
                    {
                        NewsArticleId = b.NewsArticleId,
                        NewsTitle = b.NewsTitle,
                        Headline = b.Headline,
                        CreatedDate = b.CreatedDate,
                        NewsContent = b.NewsContent,
                        NewsSource = b.NewsSource,
                        CategoryId = b.CategoryId,
                        NewsStatus = b.NewsStatus,
                        CreatedById = b.CreatedById,
                        UpdatedById = b.UpdatedById,
                        ModifiedDate = b.ModifiedDate,
                        Tags = b.Tags.Select(t => new Models.TagModel
                        {
                            TagId = t.TagId,
                            TagName = t.TagName
                        }).ToList()
                    }).ToList();

                    return View(feModel.Where(x => x.NewsStatus == true).ToList());
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
    }
}

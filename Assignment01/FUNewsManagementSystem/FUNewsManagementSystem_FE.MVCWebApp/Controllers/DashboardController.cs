using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FEMVC.Controllers
{
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://localhost:50013/api/" + "Dashboard" + "/newsCount/Category");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var newsCount = JsonConvert.DeserializeObject<int>(jsonString);
                        return View(newsCount);
                    }
                    //else
                    //{
                    //    ModelState.AddModelError("", "Failed to retrieve categories.");
                    //    return View(new List<Category>());
                    //}
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving categories: {ex.Message}");
            }
            return NotFound();
        }
    }
}

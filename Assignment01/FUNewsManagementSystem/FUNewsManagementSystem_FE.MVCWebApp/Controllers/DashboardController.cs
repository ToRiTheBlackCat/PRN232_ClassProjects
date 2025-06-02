using FEMVC.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace FEMVC.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new DashboardView();
            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync("https://localhost:50013/api/" + "Dashboard" + "/newsCount/category");
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var newsCount = JsonConvert.DeserializeObject<int>(jsonString);
                            model.FilterOptions = GetFilterOptions();
                            model.TotalNewsCount = newsCount;
                            return View(model);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error retrieving categories: {ex.Message}");
                }
            }

            // Example: set filter options
            model.FilterOptions = GetFilterOptions();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(DashboardView data)
        {
            var model = new DashboardView();
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        if (data.SelectedFilter != null) data.SelectedFilter = data.SelectedFilter.ToLower();
                        if (data.SelectedFilter == null || data.SelectedFilter.Contains("none"))
                        {
                            response = await httpClient.GetAsync("https://localhost:50013/api/" + "Dashboard" + "/newsCount/category");
                        }
                        else if (data.SelectedFilter.Contains("date"))
                        {
                            var fromDateStr = data.fromDate.ToString("O"); // ISO 8601
                            var toDateStr = data.toDate.ToString("O");
                            response = await httpClient.GetAsync("https://localhost:50013/api/" + "Dashboard" + $"/newsCount/date?fromDate={Uri.EscapeDataString(fromDateStr)}&toDate={Uri.EscapeDataString(toDateStr)}");
                        }
                        else
                        {
                            response = await httpClient.GetAsync("https://localhost:50013/api/" + "Dashboard" + "/newsCount/category?categoryName=" + data.categoryName);
                        }

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var newsCount = JsonConvert.DeserializeObject<int>(jsonString);
                            model.FilterOptions = GetFilterOptions();
                            model.TotalNewsCount = newsCount;
                            return View(model);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error retrieving categories: {ex.Message}");
                }
                // Example: set filter options 
            }
            model.FilterOptions = GetFilterOptions();
            return View(model);
        }

        private IEnumerable<SelectListItem> GetFilterOptions()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "None", Text = "None" },
            new SelectListItem { Value = "Category", Text = "Category" },
            new SelectListItem { Value = "Date", Text = "Date" }
        };
        }
    }
}
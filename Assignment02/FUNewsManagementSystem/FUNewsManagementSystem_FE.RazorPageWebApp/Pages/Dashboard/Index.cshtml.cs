using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using FUNewsManagementSystem_FE.RazorPageWebApp.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public DashboardView DashboardData { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            DashboardData = new DashboardView();
            DashboardData.FilterOptions = GetFilterOptions();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "/odata/Dashboard/newsCount");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        DashboardData.TotalNewsCount = JsonConvert.DeserializeObject<int>(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving categories: {ex.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            DashboardData.FilterOptions = GetFilterOptions();

            if (!ModelState.IsValid)
                return Page();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response;

                    var filter = DashboardData.SelectedFilter?.ToLower();

                    if (string.IsNullOrEmpty(filter) || filter.Contains("none"))
                    {
                        response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "odata/Dashboard/newsCount");
                    }
                    else if (filter.Contains("date"))
                    {
                        var fromDateStr = DashboardData.fromDate.ToString("O");
                        var toDateStr = DashboardData.toDate.ToString("O");

                        var url = ProjectConstant.APIEndPoint + $"odata/Dashboard/newsCount/?fromDate={Uri.EscapeDataString(fromDateStr)}&toDate={Uri.EscapeDataString(toDateStr)}";
                        response = await httpClient.GetAsync(url);
                    }
                    else
                    {
                        var category = Uri.EscapeDataString(DashboardData.categoryName ?? "");
                        response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + $"Dashboard/newsCount/?categoryName={category}");
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        DashboardData.TotalNewsCount = JsonConvert.DeserializeObject<int>(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving categories: {ex.Message}");
            }

            return Page();
        }

        private List<SelectListItem> GetFilterOptions()
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

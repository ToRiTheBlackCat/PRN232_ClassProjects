
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Models.FormModels
{
    public class DashboardView
    {
        public int TotalNewsCount { get; set; }
        public string? SelectedFilter { get; set; } // Fix name to match view
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string? categoryName { get; set; }
        public List<NewsArticleView> News { get; set; } = new();
        public IEnumerable<SelectListItem> FilterOptions { get; set; } = new List<SelectListItem>();
    }
}

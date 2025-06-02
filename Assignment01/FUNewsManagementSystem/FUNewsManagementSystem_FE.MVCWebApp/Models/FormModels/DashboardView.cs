using FUNewsManagementSystem_FE.MVCWebApp.Models.FormModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FEMVC.Models.FormModels
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

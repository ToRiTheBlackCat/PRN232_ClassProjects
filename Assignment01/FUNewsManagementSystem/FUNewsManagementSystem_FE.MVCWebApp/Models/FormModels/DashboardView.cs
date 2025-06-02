using FUNewsManagementSystem_FE.MVCWebApp.Models.FormModels;

namespace FEMVC.Models.FormModels
{
    public class DashboardView
    {
        public int TotalNewsCount { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string categoryName { get; set; }
        public List<NewsArticleView> News { get; set; } = new();
    }
}

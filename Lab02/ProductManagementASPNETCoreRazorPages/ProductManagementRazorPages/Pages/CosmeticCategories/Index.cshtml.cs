using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProductManagementRazorPages.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProductManagementRazorPages.Pages.CosmeticCategories.IndexModel;

namespace ProductManagementRazorPages.Pages.CosmeticCategories
{
    public class IndexModel : PageModel
    {

        public IndexModel()
        {
        }

        public IList<CosmeticCategory> CosmeticCategory { get; set; } = default!;
        public int PageSize { get; set; } = 3;
        [BindProperty(SupportsGet = true)]
        public int PageNum { get; set; } = 1;
        public int ItemCount { get; set; }
        public int PageCount => (int)Math.Ceiling(ItemCount * 1.0 / PageSize);

        // Search Properties
        [BindProperty]
        public string CategoryName { get; set; }
        [BindProperty]
        public string UsagePurpose { get; set; }
        [BindProperty]
        public string FormulationType { get; set; }

        public async Task OnGetAsync()
        {

            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var response = await APICaller.GetAsync<GetAllResponseModel<CosmeticCategory>>("CosmeticCategories", tokenString, true);

            ItemCount = response.Value.Count;
            CosmeticCategory = response.Value.Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
        }

        public async Task OnPostAsync()
        {
            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var filterQuery = "";
            if (!string.IsNullOrEmpty(CategoryName) || !string.IsNullOrEmpty(UsagePurpose) || !string.IsNullOrEmpty(FormulationType))
            {
                filterQuery = "?$filter=";
                filterQuery += !string.IsNullOrEmpty(CategoryName) ? $"startswith({nameof(CategoryName)},'{CategoryName}')" : "";
                filterQuery += !string.IsNullOrEmpty(CategoryName) && !string.IsNullOrEmpty(UsagePurpose) ? " or " : "";
                filterQuery += !string.IsNullOrEmpty(UsagePurpose) ? $"startswith({nameof(UsagePurpose)},'{UsagePurpose}')" : "";
                filterQuery += !string.IsNullOrEmpty(CategoryName + UsagePurpose) && !string.IsNullOrEmpty(FormulationType) ? " or " : "";
                filterQuery += !string.IsNullOrEmpty(FormulationType) ? $"startswith({nameof(FormulationType)},'{FormulationType}')" : "";
            }
            var response = await APICaller.GetAsync<GetAllResponseModel<CosmeticCategory>>("CosmeticCategories" + filterQuery, tokenString, true);

            PageNum = 1;
            ItemCount = response.Value.Count;
            CosmeticCategory = response.Value.Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
        }

        public class GetAllResponseModel<T>
        {
            public List<T> Value { get; set; }
        }
    }
}

using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagementRazorPages.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementRazorPages.Pages.SystemAccounts
{
    [Authorize(Roles = "1")]
    public class IndexModel : PageModel
    {
        public IndexModel()
        {

        }

        public IList<SystemAccount> SystemAccount { get; set; } = default!;

        [BindProperty]
        public int Role { get; set; } = 0;

        [BindProperty]
        public string AccountNote { get; set; }

        public async Task OnGetAsync()
        {
            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var result = await APICaller.GetAsync<GetAllModel>("SystemAccounts", tokenString, true);
            SystemAccount = result.Value;
            ViewData["Roles"] = new SelectList(AccountRoles, "Value", "Text");
        }

        public async Task OnPostAsync()
        {
            var filter = "";
            if (!string.IsNullOrEmpty(AccountNote) || Role != 0)
            {
                filter = "?$filter=";
                filter += !string.IsNullOrEmpty(AccountNote) ? $"startswith({nameof(AccountNote)},'{AccountNote}')" : "";
                filter += !string.IsNullOrEmpty(AccountNote) && Role != 0 ? " or " : "";
                filter += Role != 0 ? $"{nameof(Role)} eq {Role}" : "";
            }

            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var result = await APICaller.GetAsync<GetAllModel>("SystemAccounts" + filter, tokenString, true);
            SystemAccount = result.Value;

            ViewData["Roles"] = new SelectList(AccountRoles, "Value", "Text");
        }

        private class GetAllModel()
        {
            public List<SystemAccount> Value { get; set; }
        }


        //1-admin / 2-manager / 3-staff / 4-memeber 1
        public List<SelectListItem> AccountRoles
        {
            get => new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Admin" },
                    new SelectListItem { Value = "2", Text = "Manager" },
                    new SelectListItem { Value = "3", Text = "Staff" },
                    new SelectListItem { Value = "4", Text = "Member" },
                    new SelectListItem { Value = "0", Text = "None" },
                };
        }
    }
}

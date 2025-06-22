using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProductManagementRazorPages.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementRazorPages.Pages.SystemAccounts
{

    [Authorize(Roles = "1")]
    public class DeleteModel : PageModel
    {
        public DeleteModel()
        {
            
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var systemaccount = await APICaller.GetAsync<SystemAccount>($"SystemAccounts/{id}", tokenString, true);

            if (systemaccount == null)
            {
                return NotFound();
            }
            else
            {
                SystemAccount = systemaccount;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var deleted = await APICaller.DeleteAsync($"SystemAccounts/{id}", tokenString);

            return RedirectToPage("./Index");
        }
    }
}

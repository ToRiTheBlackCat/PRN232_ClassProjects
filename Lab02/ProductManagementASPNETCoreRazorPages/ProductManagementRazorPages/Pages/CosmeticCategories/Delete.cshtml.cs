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

namespace ProductManagementRazorPages.Pages.CosmeticCategories
{
    [Authorize(Roles = "1")]
    public class DeleteModel : PageModel
    {
        public DeleteModel()
        {
            
        }

        [BindProperty]
        public CosmeticCategory CosmeticCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var cosmeticcategory = await APICaller.GetAsync<CosmeticCategory>($"CosmeticCategories/{id}", tokenString, true);
            if (cosmeticcategory == null)
            {
                return NotFound();
            }

            CosmeticCategory = cosmeticcategory;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var result = await APICaller.DeleteAsync($"CosmeticCategories/{id}", tokenString);

            return RedirectToPage("./Index");
        }
    }
}

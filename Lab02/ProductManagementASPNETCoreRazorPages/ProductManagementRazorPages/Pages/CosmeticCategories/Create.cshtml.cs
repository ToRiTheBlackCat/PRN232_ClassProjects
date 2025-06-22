using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagementRazorPages.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementRazorPages.Pages.CosmeticCategories
{
    [Authorize(Roles = "1")]
    public class CreateModel : PageModel
    {
        public CreateModel()
        {

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CosmeticCategory CosmeticCategory { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                var cosmeticcategory = await APICaller.PostAsync<CosmeticCategory, CosmeticCategory>($"CosmeticCategories", CosmeticCategory, tokenString);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return RedirectToPage("./Index");
        }
    }
}

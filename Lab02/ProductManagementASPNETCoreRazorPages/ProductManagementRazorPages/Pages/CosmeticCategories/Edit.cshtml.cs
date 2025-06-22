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

namespace ProductManagementRazorPages.Pages.CosmeticCategories
{
    [Authorize(Roles = "1")]
    public class EditModel : PageModel
    {
        public EditModel()
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
            var cosmeticcategory =  await APICaller.GetAsync<CosmeticCategory>($"CosmeticCategories/{id}", tokenString, true);
            if (cosmeticcategory == null)
            {
                return NotFound();
            }
            CosmeticCategory = cosmeticcategory;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var updated = await APICaller.PutAsync<CosmeticCategory, CosmeticCategory>($"CosmeticCategories", CosmeticCategory);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CosmeticCategoryExists(CosmeticCategory.CategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CosmeticCategoryExists(string id)
        {
            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var cosmeticCategory = APICaller.GetAsync<CosmeticCategory>($"CosmeticCategories/{id}", tokenString, true).Result;

            return cosmeticCategory != null;
        }
    }
}

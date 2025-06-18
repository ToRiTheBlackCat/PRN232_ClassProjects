using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Models;
using DataAccessObjects;

namespace ProductManagementRazorPages.Pages.CosmeticInformations
{
    public class CreateModel : PageModel
    {
        private readonly DataAccessObjects.CosmeticsDBContext _context;

        public CreateModel(DataAccessObjects.CosmeticsDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.CosmeticCategories, "CategoryId", "CategoryId");
            return Page();
        }

        [BindProperty]
        public CosmeticInformation CosmeticInformation { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CosmeticInformations.Add(CosmeticInformation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

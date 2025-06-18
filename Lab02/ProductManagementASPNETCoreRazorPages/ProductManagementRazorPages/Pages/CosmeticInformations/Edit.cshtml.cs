using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using DataAccessObjects;

namespace ProductManagementRazorPages.Pages.CosmeticInformations
{
    public class EditModel : PageModel
    {
        private readonly DataAccessObjects.CosmeticsDBContext _context;

        public EditModel(DataAccessObjects.CosmeticsDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CosmeticInformation CosmeticInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cosmeticinformation =  await _context.CosmeticInformations.FirstOrDefaultAsync(m => m.CosmeticId == id);
            if (cosmeticinformation == null)
            {
                return NotFound();
            }
            CosmeticInformation = cosmeticinformation;
           ViewData["CategoryId"] = new SelectList(_context.CosmeticCategories, "CategoryId", "CategoryId");
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

            _context.Attach(CosmeticInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CosmeticInformationExists(CosmeticInformation.CosmeticId))
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

        private bool CosmeticInformationExists(string id)
        {
            return _context.CosmeticInformations.Any(e => e.CosmeticId == id);
        }
    }
}

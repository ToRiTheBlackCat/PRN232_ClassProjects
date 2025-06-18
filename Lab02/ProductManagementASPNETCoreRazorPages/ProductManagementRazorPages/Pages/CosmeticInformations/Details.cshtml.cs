using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using DataAccessObjects;

namespace ProductManagementRazorPages.Pages.CosmeticInformations
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccessObjects.CosmeticsDBContext _context;

        public DetailsModel(DataAccessObjects.CosmeticsDBContext context)
        {
            _context = context;
        }

        public CosmeticInformation CosmeticInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cosmeticinformation = await _context.CosmeticInformations.FirstOrDefaultAsync(m => m.CosmeticId == id);
            if (cosmeticinformation == null)
            {
                return NotFound();
            }
            else
            {
                CosmeticInformation = cosmeticinformation;
            }
            return Page();
        }
    }
}

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
    public class EditModel : PageModel
    {
        public EditModel()
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

            ViewData["Roles"] = AccountRoles;
            SystemAccount = systemaccount;
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
                var updated = await APICaller.PutAsync<SystemAccount, SystemAccount>($"SystemAccounts", SystemAccount);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemAccountExists(SystemAccount.AccountId))
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

        private bool SystemAccountExists(int id)
        {
            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
            var systemaccount = APICaller.GetAsync<SystemAccount>($"SystemAccounts/{id}", tokenString, true).Result;
            return systemaccount != null;
        }

        //1-admin / 2-manager / 3-staff / 4-memeber 1
        private SelectList AccountRoles
        {
            get => new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Admin" },
                    new SelectListItem { Value = "2", Text = "Manager" },
                    new SelectListItem { Value = "3", Text = "Staff" },
                    new SelectListItem { Value = "4", Text = "Member" },
                }, "Value", "Text");
        }
    }
}

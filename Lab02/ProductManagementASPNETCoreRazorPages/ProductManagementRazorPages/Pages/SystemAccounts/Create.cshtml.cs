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
using static ProductManagementRazorPages.Pages.CosmeticCategories.IndexModel;

namespace ProductManagementRazorPages.Pages.SystemAccounts
{

    [Authorize(Roles = "1")]
    public class CreateModel : PageModel
    {
        public CreateModel()
        {

        }

        public IActionResult OnGet()
        {
            ViewData["Roles"] = AccountRoles;
            return Page();
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;

            var filterQuery = $"?$filter={nameof(SystemAccount.EmailAddress)} eq '{SystemAccount.EmailAddress}'";
            var testExistEmail = await APICaller.GetAsync<GetAllResponseModel>("SystemAccounts" + filterQuery, tokenString, true);
            if (testExistEmail.Value.Any())
            {
                SystemAccount = default!;
                ViewData["Roles"] = AccountRoles;
                ModelState.AddModelError("SystemAccount.EmailAddress", "This email has already been taken.");
                return Page();
            }

            var created = await APICaller.PostAsync<SystemAccount, SystemAccount>($"SystemAccounts", SystemAccount, tokenString);

            return RedirectToPage("./Index");
        }

        private class GetAllResponseModel
        {
            public List<SystemAccount> Value { get; set; }
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
                }, "Value", "Text", "4");
        }
    }
}

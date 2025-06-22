using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Repository.Models;
using AutoMapper;
using FUNewsManagementSystem.Repository.Models.FormModels;
using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using Newtonsoft.Json;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;

        public IActionResult OnGet() => Page();

        [BindProperty]
        public CreateAccountForm SystemAccount { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(SystemAccount), System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(ProjectConstant.APIEndPoint + $"odata/SystemAccounts", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("./Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving Accounts: {ex.Message}");
            }
            return NotFound();
        }
    }
}

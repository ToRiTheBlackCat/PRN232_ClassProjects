using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using FUNewsManagementSystem.Repository.Models.FormModels;
using Newtonsoft.Json;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.Accounts
{
    public class DetailsModel : PageModel
    {
        public DetailsModel()
        {
        
        }

        [BindProperty]
        public SystemAccountView SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + $"odata/SystemAccounts({id})");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var item = JsonConvert.DeserializeObject<SystemAccountView>(jsonString);
                        if(item == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            SystemAccount = item;
                        }
                        return Page();
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

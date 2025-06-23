using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using Newtonsoft.Json;
using FUNewsManagementSystem_FE.RazorPageWebApp.Models.FormModels;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.Accounts
{
    public class DeleteModel : PageModel
    {
        public DeleteModel()
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
                    var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + $"odata/SystemAccounts?" + $"$filter=accountId eq {id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var item = JsonConvert.DeserializeObject<List<SystemAccountView>>(jsonString);
                        if (item == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            SystemAccount = item.First();
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

        public async Task<IActionResult> OnPostAsync(short? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync(ProjectConstant.APIEndPoint + $"api/SystemAccounts/{id}");
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

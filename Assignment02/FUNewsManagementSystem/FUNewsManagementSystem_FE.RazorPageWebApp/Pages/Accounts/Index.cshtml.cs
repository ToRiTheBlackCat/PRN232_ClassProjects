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
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        [BindProperty]
        public IList<SystemAccountView> SystemAccounts { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "odata/SystemAccounts");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<SystemAccountView>>(jsonString);
                        SystemAccounts = list;
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving Accounts: {ex.Message}");
            }
            return Page();
        }
    }
}

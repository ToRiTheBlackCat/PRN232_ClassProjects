using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using FUNewsManagementSystem_FE.RazorPageWebApp.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }
        [BindProperty]
        public string? accName { get; set; } = default!;
        [BindProperty]
        public IList<SystemAccountView> SystemAccounts { get;set; } = new List<SystemAccountView>();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
                    if (!string.IsNullOrEmpty(tokenString))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    }

                    var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "odata/SystemAccounts");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<SystemAccountView>>(jsonString);
                        SystemAccounts = list;
                        return Page();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return RedirectToPage("/Auth/Forbidden");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving Accounts: {ex.Message}");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (accName != null)
                    {
                        var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "odata/SystemAccounts?" + $"$filter=contains(accountName,'{accName}') ");
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var list = JsonConvert.DeserializeObject<List<SystemAccountView>>(jsonString);
                            SystemAccounts = list;
                            return Page();
                        }
                    }
                    else return await OnGetAsync();
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

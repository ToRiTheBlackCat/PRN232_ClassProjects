using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductManagementRazorPages.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProductManagementRazorPages.Pages.CosmeticInformations
{
    public class DeleteModel : PageModel
    {
        public DeleteModel()
        {
        }

        [BindProperty]
        public CosmeticInformation CosmeticInformation { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
                    if (!string.IsNullOrEmpty(tokenString))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    }

                    var response = await httpClient.GetAsync(ConstantVariables.APIEndPoint + "CosmeticInformations/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var item = JsonConvert.DeserializeObject<CosmeticInformation>(responseBody);

                        CosmeticInformation = item;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        return RedirectToPage("/Auth/Forbidden");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex) { }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "JwtToken").Value;
                    if (!string.IsNullOrEmpty(tokenString))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    }

                    var response = await httpClient.DeleteAsync(ConstantVariables.APIEndPoint + "CosmeticInformations/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var item = JsonConvert.DeserializeObject<CosmeticInformation>(responseBody);

                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        return RedirectToPage("/Auth/Forbidden");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex) { }

            return RedirectToPage("./Index");
        }
    }
}

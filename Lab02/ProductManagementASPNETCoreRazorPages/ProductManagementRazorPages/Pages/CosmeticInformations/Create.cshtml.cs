using Azure;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProductManagementRazorPages.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementRazorPages.Pages.CosmeticInformations
{
    public class CreateModel : PageModel
    {

        public CreateModel()
        {
            
        }

        public async  Task<IActionResult> OnGet()
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

                    var cateResponse = await httpClient.GetAsync(ConstantVariables.APIEndPoint + "CosmeticCategories");
                    if (cateResponse.IsSuccessStatusCode)
                    {
                        var result = await cateResponse.Content.ReadAsStringAsync();
                        var cateList = JsonConvert.DeserializeObject<List<CosmeticCategory>>(result);

                        ViewData["CategoryId"] = new SelectList(cateList, "CategoryId", "CategoryName");
                    }
                    else if (cateResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        return RedirectToPage("/Auth/Forbidden");
                    }
                }
            }
            catch (Exception ex) { }

            return Page();
        }

        [BindProperty]
        public CosmeticInformation CosmeticInformation { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
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

                    var jsonContent = JsonConvert.SerializeObject(CosmeticInformation);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(ConstantVariables.APIEndPoint + "CosmeticInformations/", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<CosmeticInformation>(responseBody);

                        return RedirectToPage("./Index");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        return RedirectToPage("/Auth/Forbidden");
                    }
                    else
                    {
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToPage("./Index");
        }
    }
}

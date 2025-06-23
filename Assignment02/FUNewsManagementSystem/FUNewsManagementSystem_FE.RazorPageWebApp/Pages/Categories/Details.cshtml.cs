using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem_FE.RazorPageWebApp.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.Categories
{
    public class DetailsModel : PageModel
    {

        public DetailsModel()
        {

        }

        public Category Category { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(short? id)
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

                    var response = await httpClient.GetAsync(ConstantVariables.APIEndPoint + "Categories/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var item = JsonConvert.DeserializeObject<Category>(responseBody);

                        Category = item;
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
    }
}

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
    public class IndexModel : PageModel
    {//Categories

        public IndexModel()
        {
        }

        public IList<Category> Category { get; set; } = new List<Category>();

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

                    var response = await httpClient.GetAsync(ConstantVariables.APIEndPoint + "Categories");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var categories = JsonConvert.DeserializeObject<List<Category>>(jsonString);

                        Category = categories;

                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return RedirectToPage("/Auth/Forbidden");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving categories: {ex.Message}");
            }
            return Page();
        }
    }
}

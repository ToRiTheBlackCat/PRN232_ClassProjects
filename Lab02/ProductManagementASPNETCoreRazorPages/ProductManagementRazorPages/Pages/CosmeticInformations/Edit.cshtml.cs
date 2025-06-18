using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using ProductManagementRazorPages.Constant;
using ProductManagementWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementRazorPages.Pages.CosmeticInformations
{
    public class EditModel : PageModel
    {

        public EditModel()
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

                        var cateResponse = await httpClient.GetAsync(ConstantVariables.APIEndPoint + "CosmeticCategories");
                        if (cateResponse.IsSuccessStatusCode)
                        {
                            var result = await cateResponse.Content.ReadAsStringAsync();
                            var cateList = JsonConvert.DeserializeObject<List<CosmeticCategory>>(result);

                            ViewData["CategoryId"] = new SelectList(cateList, "CategoryId", "CategoryName");

                        }
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



        // To protect from overposting attacks, enable the specific properties you want to bind to.
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

                    var response = await httpClient.PutAsync(ConstantVariables.APIEndPoint + "CosmeticInformations/" + CosmeticInformation.CosmeticId, content);

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

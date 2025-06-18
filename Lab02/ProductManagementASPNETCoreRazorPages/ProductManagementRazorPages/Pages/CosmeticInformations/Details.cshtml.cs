using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using DataAccessObjects;
using Newtonsoft.Json;
using ProductManagementRazorPages.Constant;

namespace ProductManagementRazorPages.Pages.CosmeticInformations
{
    public class DetailsModel : PageModel
    {

        public DetailsModel()
        {

        }

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
                    //var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);


                    var response = await httpClient.GetAsync(ConstantVariables.APIEndPoint + "CosmeticInformations/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var item = JsonConvert.DeserializeObject<CosmeticInformation>(responseBody);

                        CosmeticInformation = item;
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

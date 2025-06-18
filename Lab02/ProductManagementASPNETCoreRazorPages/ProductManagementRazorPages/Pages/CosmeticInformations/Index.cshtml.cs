using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using ProductManagementRazorPages.Constant;
using ProductManagementWebAPI;
using System.Text;

namespace ProductManagementRazorPages.Pages.CosmeticInformations
{
    public class IndexModel : PageModel
    {

        public IndexModel()
        {
        }

        public IList<CosmeticInformation> CosmeticInformation { get; set; } = new List<CosmeticInformation>();

        public async Task OnGetAsync()
        {

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);


                    var response = await httpClient.GetAsync(ConstantVariables.APIEndPoint + "CosmeticInformations");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<CosmeticInformation>>(responseBody);

                        CosmeticInformation = list;
                    }
                }

            }
            catch (Exception ex) { }
        }
    }

}

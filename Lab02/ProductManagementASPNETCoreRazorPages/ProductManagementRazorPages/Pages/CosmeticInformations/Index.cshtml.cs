using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class IndexModel : PageModel
    {

        public IndexModel()
        {
        }

        public IList<CosmeticInformation> CosmeticInformation { get; set; } = new List<CosmeticInformation>();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(ConstantVariables.APIEndPoint + "CosmeticInformations");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<CosmeticInformation>>(responseBody);

                        CosmeticInformation = list;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        return RedirectToPage("/Auth/Forbidden");
                    }
                }

            }
            catch (Exception ex) { }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string searchName)
        {
            if (searchName == null)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync(ConstantVariables.APIEndPoint + "CosmeticInformations");
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();
                            var list = JsonConvert.DeserializeObject<List<CosmeticInformation>>(responseBody);

                            CosmeticInformation = list;
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            return RedirectToPage("/Auth/Forbidden");
                        }
                    }
                }
                catch (Exception ex) { }
            }
            else
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        string url = $"https://localhost:7252/odata/CosmeticInformations?$filter=startswith(CosmeticName,%27{searchName}%27)";

                        var response = await httpClient.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();
                            var wrapper = JsonConvert.DeserializeObject<ODataResponse<CosmeticInformation>>(responseBody);
                            CosmeticInformation = wrapper?.Value ?? new List<CosmeticInformation>();
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            return RedirectToPage("/Auth/Forbidden");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Optional: add logging
                }
            }

            return Page();
        }
        public class ODataResponse<T>
        {
            public List<T> Value { get; set; }
        }

    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Newtonsoft.Json;
using FUNewsManagementSystem_FE.RazorPageWebApp.Constant;
using FUNewsManagementSystem_BE.API.Models;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.Auth
{
    
    public class LoginModel : PageModel
    {
        

        public LoginModel(IConfiguration configuration)
        {
            
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.LoginRequest Login { get; set; } = default!;
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
                    var jsonContent = JsonConvert.SerializeObject(Login);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(ConstantVariables.APIEndPoint + "Accounts/login", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<AccountResponseDTO>(responseBody);

                        if (apiResponse != null && apiResponse.Token != null)
                        {
                            Response.Cookies.Append("JwtToken", apiResponse.Token);
                            Response.Cookies.Append("Fullname", apiResponse.AccountName);

                            return RedirectToPage("/Categories/Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid email or password. Please try again.");
                            return Page();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ModelState.AddModelError("", "Login failure. Invalid email or password");

            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            Response.Cookies.Delete("JwtToken");
            Response.Cookies.Delete("Fullname");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Auth/Login");
        }

        public async Task<IActionResult> Forbidden()
        {
            return RedirectToPage("/Auth/Forbidden");
        }
    }
}

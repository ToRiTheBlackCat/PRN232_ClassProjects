using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace FUNewsManagementSystem_FE.MVCWebApp.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string AccountEmail, string AccountPassword)
        {
            var loginRequest = new
            {
                AccountEmail,
                AccountPassword
            };

            try
            {
                Console.WriteLine("Before sending request");
                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("Inside httpClient using");
                    var jsonContent = JsonConvert.SerializeObject(loginRequest);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    using (
                        var response = await httpClient.PostAsync(ProjectConstant.APIEndPoint + "api/Accounts/login", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();
                            var account = JsonConvert.DeserializeObject<SystemAccount>(responseBody);

                            if (account != null)
                            {
                                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, account.AccountName),
                            new Claim(ClaimTypes.Role, account.AccountRole.ToString())
                        };

                                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                                Response.Cookies.Append("FullName", account.AccountName);
                                Response.Cookies.Append("Role", account.AccountRole.ToString());

                                return RedirectToAction("Index", "Categories");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
    }
}

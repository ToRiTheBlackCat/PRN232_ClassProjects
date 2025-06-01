using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductManagementMVC.Constant;

using BusinessObjects;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ProductManagementMVC.Controllers
{
    public class AuthController : Controller
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
        public async Task<IActionResult> Login(string EmailAddress, string MemberPassword)
        {
            var loginRequest = new
            {
                EmailAddress,
                MemberPassword
            };

            using (var httpClient = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(ApiUrlConstant.APIEndPoint + "Accounts/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var account = JsonConvert.DeserializeObject<AccountMember>(responseBody);

                    if (account != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, account.FullName),
                            new Claim(ClaimTypes.Role, account.MemberRole.ToString())
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                        Response.Cookies.Append("FullName", account.FullName);
                        Response.Cookies.Append("Role", account.MemberRole.ToString());

                        return RedirectToAction("Index", "Products");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid email or password. Please try again.");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("FullName");
            Response.Cookies.Delete("Role");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}

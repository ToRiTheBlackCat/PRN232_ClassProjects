using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using FUNewsManagementSystem_FE.MVCWebApp.Models;


namespace FUNewsManagementSystem_FE.MVCWebApp.Controllers
{
    public class AdminAccountConfig
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class AccountsController : Controller
    {
        private readonly AdminAccountConfig _adminConfig;

        public AccountsController(IConfiguration configuration)
        {
            _adminConfig = configuration.GetSection("AdminAccount").Get<AdminAccountConfig>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (request.AccountEmail == _adminConfig.Email && request.AccountPassword == _adminConfig.Password)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _adminConfig.Email),
                        new Claim(ClaimTypes.Role, _adminConfig.Role)  
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                Response.Cookies.Append("FullName", _adminConfig.Email);
                Response.Cookies.Append("Role", _adminConfig.Role);

                return RedirectToAction("Index", "Accounts");
            }
            try
            {
                using (var httpClient = new HttpClient())
                {
                    ;
                    var jsonContent = JsonConvert.SerializeObject(request);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    using (
                        var response = await httpClient.PostAsync(ProjectConstant.APIEndPoint + "Accounts/signin", content))
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

                                return RedirectToAction("Index", "Home");
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("FullName");
            Response.Cookies.Delete("Role");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("Login");
        }

        public async Task<IActionResult> Forbidden()
        {
            return View();
        }
    }
}

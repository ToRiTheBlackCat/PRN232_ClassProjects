using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductManagementMVC.Constant;

using BusinessObjects;
using System.Text;

namespace ProductManagementMVC.Controllers
{
    public class AccountController : Controller
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
                // Serialize the request object
                var jsonContent = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                using (var response = await httpClient.PostAsync(ApiUrlConstant.APIEndPoint + "Accounts/login", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var account = JsonConvert.DeserializeObject<AccountMember>(responseBody);

                        if (account != null)
                        {
                            return RedirectToAction("Index", "Home"); 
                        }
                    }
                }
            }

            ModelState.AddModelError("", "Not found any account with that email or password. Please try again");
            return View();
        }
    }
}

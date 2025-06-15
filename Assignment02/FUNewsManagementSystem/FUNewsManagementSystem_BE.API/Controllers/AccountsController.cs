using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = FUNewsManagementSystem_BE.API.Models.LoginRequest;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("signin")]
        public async Task<ActionResult<SystemAccount>> Signin(LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.AccountEmail) || string.IsNullOrEmpty(loginRequest.AccountPassword)) {
                return BadRequest("Invalid email or password");
            }
            var account = await _accountService.Authenticate(loginRequest.AccountEmail, loginRequest.AccountPassword);
            if (account == null)
            {
                return BadRequest("No account found");
            }
            else
            {
                CookieOptions cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    HttpOnly = true
                };
                Response.Cookies.Append("UserEmail", account.AccountEmail, cookieOptions);
                return Ok(account);
            }
                
        }
    }
}

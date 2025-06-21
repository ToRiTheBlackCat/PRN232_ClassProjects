using Azure.Core;
using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Service;
using FUNewsManagementSystem_BE.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = FUNewsManagementSystem_BE.API.Models.LoginRequest;

namespace FUNewsManagementSystem_BE.API.Controllers
{

    public class AdminAccountConfig
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly AdminAccountConfig _adminConfig;
        public AccountsController(IAccountService accountService, IConfiguration config)
        {
            _accountService = accountService;
            _adminConfig = config.GetSection("AdminAccount").Get<AdminAccountConfig>();

        }
        [HttpPost("login")]
        public async Task<ActionResult<SystemAccount>> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", true, true).Build();
                var account = new SystemAccount();

                //Check admin account
                if (loginRequest.Email == _adminConfig.Email && loginRequest.Password == _adminConfig.Password)
                {
                    account.AccountEmail = loginRequest.Email;
                    account.AccountPassword = loginRequest.Password;
                    account.AccountRole = _adminConfig.Role;

                }
                else
                {
                    account = await _accountService.Authenticate(loginRequest.Email, loginRequest.Password);
                    if (account == null)
                    {
                        return Unauthorized("Invalid email or password.");
                    }
                }

                //Create Claims
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, account.AccountEmail),
                        new Claim(ClaimTypes.Role, account.AccountRole.ToString()),
                        new Claim("AccountId",account.AccountId.ToString())
                    };

                var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                var signCredential = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);

                var preparedToken = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(16),
                    signingCredentials: signCredential);

                var generatedToken = new JwtSecurityTokenHandler().WriteToken(preparedToken);
                var role = account.AccountRole.ToString();
                var accountName = account.AccountName ?? "Admin";

                return Ok(new AccountResponseDTO
                {
                    Role = role,
                    Token = generatedToken,
                    AccountName = accountName
                });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

    }

}

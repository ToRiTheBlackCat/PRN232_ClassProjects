using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1")]
    public class SystemAccountsController : ODataController
    {
        private readonly ISystemAccountService _systemAccountService;

        public SystemAccountsController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }


        // POST: api/SystemAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] AccountRequestDTO loginDTO)
        {
            try
            {
                var account = await _systemAccountService.Login(loginDTO.Email, loginDTO.Password);
                if (account == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true).Build();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, account.EmailAddress),
                    new Claim(ClaimTypes.Role, account.Role.ToString()),
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
                var role = account.Role.ToString();
                var accountName = account.AccountNote.ToString();

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

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemAccount>>> Get()
        {
            return await _systemAccountService.Get();
        }


        [HttpGet("{key}")]
        public async Task<ActionResult<SystemAccount>> GetAsync([FromRoute] int key)
        {
            var cosmeticCategory = await _systemAccountService.Get(key);

            if (cosmeticCategory == null)
            {
                return NotFound();
            }

            return cosmeticCategory;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount([FromBody] SystemAccount cosmeticCategory)
        {
            try
            {
                var result = await _systemAccountService.Update(cosmeticCategory);
                return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(cosmeticCategory.AccountId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        // POST: api/CosmeticCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CosmeticCategory>> CreateAccount([FromBody] SystemAccount account)
        {
            try
            {
                var result = await _systemAccountService.Add(account);
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.AccountId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        // DELETE: api/CosmeticCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if (!AccountExists(id))
            {
                return NotFound();
            }

            try
            {
                var result = await _systemAccountService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        private bool AccountExists(int id)
        {
            var item = _systemAccountService.Get(id).Result;
            return item != null;
        }
    }
}

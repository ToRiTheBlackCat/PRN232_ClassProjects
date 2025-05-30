using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObjects;
using Services;
using Microsoft.AspNetCore.Identity.Data;
using ProductManagementMVC.Models;
using LoginRequest = ProductManagementMVC.Models.LoginRequest;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _userService;

        public AccountsController(IAccountService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AccountMember>> LoginAsync(LoginRequest request)
        {
            try
            {
                var foundAccount = _userService.Authenticate(request);
                if (foundAccount != null)
                {
                    return Ok(foundAccount); 
                }
            }
            catch (Exception ex)
            {
                // Optionally log the error
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            return NotFound("Invalid email or password");
        }


        // GET: api/Account
        [HttpGet]
        public ActionResult<IEnumerable<AccountMember>> GetAccountMembers()
        {
            return _userService.GetAccounts();
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public ActionResult<AccountMember> GetAccountMember(string id)
        {
            var accountMember = _userService.GetAccountById(id);

            if (accountMember == null)
            {
                return NotFound();
            }

            return accountMember;
        }

        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutAccountMember(string id, AccountMember accountMember)
        {
            if (id != accountMember.MemberId)
            {
                return BadRequest();
            }

            try
            {
                _userService.UpdateAccount(accountMember);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountMemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Account
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<AccountMember> PostAccountMember(AccountMember accountMember)
        {
            _userService.SaveAccount(accountMember);

            return CreatedAtAction("GetAccountMember", new { id = accountMember.MemberId }, accountMember);
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAccountMember(string id)
        {
            var accountMember = _userService.GetAccountById(id);
            if (accountMember == null)
            {
                return NotFound();
            }

            _userService.DeleteAccount(accountMember);

            return NoContent();
        }

        private bool AccountMemberExists(string id)
        {
            var foundAccount = _userService.GetAccountById(id);
            return foundAccount != null ? true : false;
        }
    }
}

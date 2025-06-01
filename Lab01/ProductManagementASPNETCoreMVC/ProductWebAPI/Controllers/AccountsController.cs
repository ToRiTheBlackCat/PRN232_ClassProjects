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
using BusinessObjects.ViewModel.Accounts;
using System.Text.Json;

namespace ProductWebAPI.Controllers
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

        // GET: api/Account
        [HttpGet]
        public ActionResult<PaginatedAccountMembers> GetAccountMembers(string fullName = "", string email = "", int roleId = 0, int pageNum = 1)
        {
            var result = _accountService.SearchAccount(fullName, email, roleId, pageNum);

            return result;
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public ActionResult<AccountMember> GetAccountMember(string id)
        {
            var accountMember = _accountService.GetAccountById(id);

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
                _accountService.UpdateAccount(accountMember);
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
            _accountService.SaveAccount(accountMember);

            return CreatedAtAction("GetAccountMember", new { id = accountMember.MemberId }, accountMember);
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAccountMember(string id)
        {
            var accountMember = _accountService.GetAccountById(id);
            if (accountMember == null)
            {
                return NotFound();
            }

            _accountService.DeleteAccount(accountMember);

            return NoContent();
        }

        private bool AccountMemberExists(string id)
        {
            var foundAccount = _accountService.GetAccountById(id);
            return foundAccount != null ? true : false;
        }
    }
}

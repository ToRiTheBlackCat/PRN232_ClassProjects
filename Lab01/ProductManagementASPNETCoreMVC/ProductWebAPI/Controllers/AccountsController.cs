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

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _context;

        public AccountsController(IAccountService context)
        {
            _context = context;
        }

        // GET: api/Account
        [HttpGet]
        public ActionResult<IEnumerable<AccountMember>> GetAccountMembers()
        {
            return _context.GetAccounts();
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public ActionResult<AccountMember> GetAccountMember(string id)
        {
            var accountMember = _context.GetAccountById(id);

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
                _context.UpdateAccount(accountMember);
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
            _context.SaveAccount(accountMember);

            return CreatedAtAction("GetAccountMember", new { id = accountMember.MemberId }, accountMember);
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAccountMember(string id)
        {
            var accountMember = _context.GetAccountById(id);
            if (accountMember == null)
            {
                return NotFound();
            }

            _context.DeleteAccount(accountMember);

            return NoContent();
        }

        private bool AccountMemberExists(string id)
        {
            var foundAccount = _context.GetAccountById(id);
            return foundAccount != null ? true : false;
        }
    }
}

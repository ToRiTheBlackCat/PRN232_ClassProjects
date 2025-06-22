using AutoMapper;
using AutoMapper.QueryableExtensions;
using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Repository.Models.FormModels;
using FUNewsManagementSystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    [Route("odata/SystemAccounts")]
    [ApiController]
    public class OData_SystemAccountsController : ControllerBase
    {
        private readonly AccountService _accServ;
        private readonly IMapper _mapper;

        public OData_SystemAccountsController(AccountService accServ, IMapper mapper)
        {
            _accServ = accServ;
            _mapper = mapper;
        }

        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<SystemAccountView>> Get()
        {
            var mapped = _accServ.GetAccountsQuery()
                     .ProjectTo<SystemAccountView>(_mapper.ConfigurationProvider);
            return Ok(mapped);
        }

        [EnableQuery]
        public async Task<SingleResult<SystemAccountView>> Get([FromODataUri] short key)
        {
            var result = await _accServ.GetAccountByIdAsync(key);
            var mapped = new[] { result }.AsQueryable().ProjectTo<SystemAccountView>(_mapper.ConfigurationProvider);

            return SingleResult.Create(mapped);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountForm form)
        {
            var result = await _accServ.CreateAccountAsync(_mapper.Map<SystemAccount>(form));
            switch (result)
            {
                default:
                    var item = await _accServ.GetAccountByNameAsync(form.AccountName);
                    return Ok(item);

                case 0:
                    return BadRequest(new
                    {
                        Message = "Email or Name already existed!",
                    });
                case -1:
                    return NotFound(new
                    {
                        Message = "Account not found!",
                    });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromODataUri]short id)
        {
            return !(await _accServ.DeleteAccountAsync(id))
                ? Problem(
                    "Delete unsuccessful!"
                )
                : Ok(new
                {
                    Message = "Delete successful!"
                });
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateAccount(UpdateAccountForm acc)
        {
            return await _accServ.UpdateAccountAsync(_mapper.Map<SystemAccount>(acc)) < 0
                ? Problem(
                    "Update unsuccessful!"
                )
                : Ok(new
                {
                    Message = "Update successful!"
                });
        }
    }
}

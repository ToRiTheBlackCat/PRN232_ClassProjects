using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Service;
using FUNewsManagementSystem.Repository.Models.FormModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagementSystem_BE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemAccountsController : Controller
    {
        private readonly AccountService _serv;
        private readonly IMapper _mapper;

        public SystemAccountsController(AccountService serv, IMapper mapper)
        {
            _serv = serv;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListAll()
        {
            var item = await _serv.ListAccountsAsync("");
            return item == null
                ? NotFound(new
                {
                    Message = "No account with that name exists (or no account in database)"
                })
                : Ok(_mapper.Map<List<SystemAccountView>>(item));
        }

        [HttpGet("name/{accountName}")]
        public async Task<IActionResult> ListByName(string? accountName)
        {
            var item = await _serv.ListAccountsAsync(accountName);
            return item == null
                ? NotFound(new
                {
                    Message = "No account with that name exists (or no account in database)"
                })
                : Ok(_mapper.Map<List<SystemAccountView>>(item));
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> SearchById(short id)
        {
            if (id == null)
            {
                return BadRequest(new
                {
                    Message = "Please provide an Id for search!",
                });
            }

            var systemAccount = await _serv.GetAccountByIdAsync(id);
            return systemAccount == null
                ? NotFound(new
                {
                    Message = $"Account with ID {id} not found!",
                })
                : Ok(_mapper.Map<SystemAccountView>(systemAccount));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountForm form)
        {
            var result = await _serv.CreateAccountAsync(_mapper.Map<SystemAccount>(form));
            switch (result)
            {
                default:
                    var item = await _serv.GetAccountByNameAsync(form.AccountName);
                    return Ok(item);

                case 0: return BadRequest(new
                {
                    Message = "Email or Name already existed!",
                });
                case -1: return NotFound(new
                {
                    Message = "Account not found!",
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(short id)
        {
            return !(await _serv.DeleteAccountAsync(id))
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
            return await _serv.UpdateAccountAsync(_mapper.Map<SystemAccount>(acc)) < 0
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
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
        public async Task<IActionResult> ListAllOrByName(string? accountName)
        {
            var item = await _serv.ListAccountsAsync(accountName);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SearchById(short id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemAccount = await _serv.GetAccountByIdAsync(id);
            return systemAccount == null
                ? NotFound()
                : Ok(systemAccount);
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

                case 0: return BadRequest();
                case -1: return NotFound();
            }
        }
    }
}
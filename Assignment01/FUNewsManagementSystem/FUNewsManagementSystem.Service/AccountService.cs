using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Service
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;
        public AccountService() => _accountRepository = new AccountRepository();

        public async Task<SystemAccount?> Authenticate(string email, string password)
        {
            return await _accountRepository.GetSystemAccount(email, password);
        }
    }
}

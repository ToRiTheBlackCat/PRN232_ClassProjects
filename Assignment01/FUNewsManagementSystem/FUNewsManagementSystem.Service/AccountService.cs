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
        private readonly AccountRepository _repo;

        public AccountService(AccountRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<SystemAccount>> ListAccountsAsync(string accName)
        {
            return await _repo.GetAccountsAsync(accName);
        }
    }
}

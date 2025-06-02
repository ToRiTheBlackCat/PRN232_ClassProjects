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
        public AccountService(AccountRepository repo)
        {
            _repo = repo;
        }

        public async Task<SystemAccount?> GetAccountByIdAsync(short accId)
        {
            return await _repo.GetAccountByIdAsync(accId);
        }

        public async Task<SystemAccount?> GetAccountByNameAsync(string? accName)
        {
            return await _repo.GetAccountByNameAsync(accName);
        }

        public async Task<List<SystemAccount>> ListAccountsAsync(string? accName)
        {
            return await _repo.GetAccountsAsync(accName);
        }

        public async Task<int> UpdateAccountAsync(SystemAccount acc)
        {
            return await _repo.UpdateAccountAsync(acc);
        }

        public async Task<bool> DeleteAccountAsync(short id)
        {
            return await _repo.DeleteAccountAsync(id);
        }

        public async Task<int> CreateAccountAsync(SystemAccount acc)
        {
            return await _repo.CreateAccountAsync(acc);
        }
    }
}
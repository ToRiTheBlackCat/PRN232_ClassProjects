using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Service
{
    public interface IAccountService
    {
        Task<SystemAccount?> Authenticate(string email, string password);
    }

    public class AccountService : IAccountService
    {
        private readonly AccountRepository _accountRepository;
        public AccountService() => _accountRepository = new AccountRepository();

        public async Task<SystemAccount?> GetAccountByIdAsync(short accId)
        {
            return await _accountRepository.GetAccountByIdAsync(accId);
        }

        public async Task<SystemAccount?> Authenticate(string email, string password)
        {
            return await _accountRepository.GetSystemAccount(email, password);
        }

        public async Task<SystemAccount?> GetAccountByNameAsync(string? accName)
        {
            return await _accountRepository.GetAccountByNameAsync(accName);
        }

        public async Task<List<SystemAccount>> ListAccountsAsync(string? accName)
        {
            return await _accountRepository.GetAccountsAsync(accName);
        }

        public async Task<int> UpdateAccountAsync(SystemAccount acc)
        {
            return await _accountRepository.UpdateAccountAsync(acc);
        }

        public async Task<bool> DeleteAccountAsync(short id)
        {
            return await _accountRepository.DeleteAccountAsync(id);
        }

        public async Task<int> CreateAccountAsync(SystemAccount acc)
        {
            return await _accountRepository.CreateAccountAsync(acc);
        }
    }
}
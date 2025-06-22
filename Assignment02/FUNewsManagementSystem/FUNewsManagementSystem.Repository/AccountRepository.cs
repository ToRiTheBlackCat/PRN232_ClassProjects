using FUNewsManagementSystem.Repository.Base;
using FUNewsManagementSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Repository
{
    public class AccountRepository : GenericRepository<SystemAccount>
    {
        public AccountRepository()
        {
        }

        public async Task<bool> RemoveWithCheckingAsync(SystemAccount acc)
        {
            if (acc.NewsArticles == null || acc.NewsArticles.Count <= 0)
            {
                _context.Remove(acc);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<SystemAccount?> GetAccountByIdAsync(short accId)
        {
            return await _context.SystemAccounts.Include(x => x.NewsArticles).Where(x => x.AccountId == accId).FirstOrDefaultAsync();
        }

        public async Task<SystemAccount?> GetAccountByNameAsync(string? accName)
        {
            return await _context.SystemAccounts.Include(x => x.NewsArticles).Where(x => x.AccountName == accName).FirstOrDefaultAsync();
        }

        public async Task<List<SystemAccount>> GetAccountsAsync(string? accName)
        {
            if (accName != null)
            {
                return await _context.SystemAccounts.Include(x => x.NewsArticles).Where(x => x.AccountName.Contains(accName)).ToListAsync();
            }
            return await _context.SystemAccounts.Include(x => x.NewsArticles).ToListAsync();
        }

        public async Task<int> UpdateAccountAsync(SystemAccount acc)
        {
            SystemAccount existingAcc = await _context.SystemAccounts.Where(x => x.AccountId == acc.AccountId).FirstOrDefaultAsync();
            if (existingAcc != null)
            {
                return await UpdateAsync(acc);
            }
            return -1; //Account not found
        }

        public async Task<bool> DeleteAccountAsync(short id)
        {
            SystemAccount? existingAcc = await _context.SystemAccounts.Include(x => x.NewsArticles).FirstOrDefaultAsync(x => x.AccountId == id);
            if (existingAcc != null)
            {
                return await RemoveWithCheckingAsync(existingAcc);
            }
            return false; //Account not found
        }

        public async Task<int> CreateAccountAsync(SystemAccount acc)
        {
            SystemAccount? existingAccount = await _context.SystemAccounts
                .FirstOrDefaultAsync(x => (x.AccountName == acc.AccountName)||(x.AccountEmail == acc.AccountEmail));
            if (existingAccount == null)
            {
                short count = (short)(_context.SystemAccounts.Max(x => x.AccountId) + 1);
                acc.AccountId = count;
                return await CreateAsync(acc);
            }
            else if (existingAccount != null) return 0; //Email or Name already existed
            else return -1; //Unknown error
        }

        public async Task<SystemAccount?> GetSystemAccount (string email, string password)
        {
            var account = await _context.SystemAccounts.FirstOrDefaultAsync(u => u.AccountEmail == email && u.AccountPassword == password);
            return account;
        }

        public IQueryable<SystemAccount> GetAccountsQuery()
        {
            return _context.SystemAccounts.Include(x => x.NewsArticles).AsQueryable();
        }
    }
}
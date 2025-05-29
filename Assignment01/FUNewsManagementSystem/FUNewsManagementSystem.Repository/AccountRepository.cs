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
            if (acc.NewsArticles == null)
            {
                _context.Remove(acc);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<SystemAccount>> GetAccountsAsync(string? accName)
        {
            if (accName != null)
            {
                return await _context.SystemAccounts.Where(x => x.AccountName.Contains(accName)).ToListAsync();
            }
            return await _context.SystemAccounts.ToListAsync();
        }
        public async 
    }
}
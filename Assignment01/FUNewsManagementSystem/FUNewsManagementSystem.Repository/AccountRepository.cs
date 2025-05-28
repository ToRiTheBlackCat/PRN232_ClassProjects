using FUNewsManagementSystem.Repository.Base;
using FUNewsManagementSystem.Repository.Models;
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
    }
}

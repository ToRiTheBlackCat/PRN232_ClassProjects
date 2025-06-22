using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO? instance = null;

        private SystemAccountDAO() { }

        public static SystemAccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemAccountDAO();
                }
                return instance;
            }
        }

        public async Task<SystemAccount> Login(string email, string password)
        {
            using (var context = new CosmeticsDBContext())
            {
                var account = await context.SystemAccounts.FirstOrDefaultAsync(account => account.EmailAddress == email && account.AccountPassword == password);
                return account;
            }
        }

        public async Task<List<SystemAccount>> GetAll()
        {
            using (var context = new CosmeticsDBContext())
            {
                var accounts = await context.SystemAccounts.ToListAsync();
                return accounts;
            }
        }

        public async Task<SystemAccount> GetById(int id)
        {
            using (var context = new CosmeticsDBContext())
            {
                var accounts = await context.SystemAccounts.FirstOrDefaultAsync(x => x.AccountId.Equals(id));
                return accounts;
            }
        }

        public async Task<SystemAccount> Add(SystemAccount category)
        {
            using (var context = new CosmeticsDBContext())
            {
                category.AccountId = GenerateId();
                await context.SystemAccounts.AddAsync(category);
                await context.SaveChangesAsync();
                return category;
            }
        }

        public async Task<SystemAccount> Update(SystemAccount category)
        {
            using (var context = new CosmeticsDBContext())
            {
                context.SystemAccounts.Update(category);
                await context.SaveChangesAsync();
                return category;
            }
        }

        public async Task<SystemAccount> Delete(int id)
        {
            using (var context = new CosmeticsDBContext())
            {
                var account = await context.SystemAccounts.FirstOrDefaultAsync(x => x.AccountId.Equals(id));
                if (account == null)
                {
                    throw new Exception("Category not found.");
                }

                context.SystemAccounts.Remove(account);
                await context.SaveChangesAsync();
                return account;
            }
        }

        private int GenerateId()
        {
            using (var context = new CosmeticsDBContext())
            {
                var account = context.SystemAccounts.ToList();
                if (account == null)
                {
                    return 0;
                }

                var maxId = account.OrderByDescending(x => x.AccountId).First();
                return maxId.AccountId + 1;
            }
        }
    }
}

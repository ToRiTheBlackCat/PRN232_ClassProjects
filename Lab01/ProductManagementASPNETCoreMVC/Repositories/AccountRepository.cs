using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public void CreateAccount(AccountMember acc) => AccountDAO.CreateAccount(acc);

        public void DeleteAccount(AccountMember acc) => AccountDAO.DeleteAccount(acc);

        public AccountMember? GetAccountById(string accountId) => AccountDAO.GetAccountById(accountId);

        public List<AccountMember> GetAccounts() => AccountDAO.GetAccounts();

        public void SaveAccount(AccountMember acc) => AccountDAO.SaveAccount(acc);

        public void UpdateAccount(AccountMember acc) => AccountDAO.UpdateAccount(acc);

        public List<AccountMember> SearchAccount(string fullName, string email, int roleId) => AccountDAO.SearchAccount(fullName, email, roleId);
    }
}

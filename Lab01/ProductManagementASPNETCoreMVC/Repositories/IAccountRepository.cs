using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAccountRepository
    {
        void SaveAccount(AccountMember acc);
        void DeleteAccount(AccountMember acc);
        void UpdateAccount(AccountMember acc);
        void CreateAccount(AccountMember acc);
        List<AccountMember> GetAccounts();
        AccountMember? GetAccountById(string accountId);
        AccountMember? GetAccountByEmailAndPassword(string email, string password);

        List<AccountMember> SearchAccount(string fullName, string email, int roleId);
    }
}

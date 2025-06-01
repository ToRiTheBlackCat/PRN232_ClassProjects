using BusinessObjects;
using ProductManagementMVC.Models;
using BusinessObjects.ViewModel.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAccountService
    {
        void SaveAccount(AccountMember acc);
        void DeleteAccount(AccountMember acc);
        void UpdateAccount(AccountMember acc);
        AccountMember? GetAccountById(string accountId);
        AccountMember? Authenticate(LoginRequest request);

        List<AccountMember> GetAccounts();
        PaginatedAccountMembers SearchAccount(string fullName, string email, int roleId, int pageNum);
    }
}

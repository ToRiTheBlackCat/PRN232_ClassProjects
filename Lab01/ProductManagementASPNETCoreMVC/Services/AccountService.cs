using BusinessObjects;
using ProductManagementMVC.Models;
using BusinessObjects.ViewModel.Accounts;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository iAccountRepository;
        public AccountService()
        {
            iAccountRepository = new AccountRepository();
        }

        public AccountMember? Authenticate(LoginRequest request)
        {
            return iAccountRepository.GetAccountByEmailAndPassword(request.EmailAddress, request.MemberPassword);
        }

        public void DeleteAccount(AccountMember acc)
        {
            iAccountRepository.DeleteAccount(acc);
        }

        public AccountMember? GetAccountById(string accountId)
        {
            return iAccountRepository.GetAccountById(accountId);
        }

        public List<AccountMember> GetAccounts()
        {
            return iAccountRepository.GetAccounts();
        }

        public void SaveAccount(AccountMember acc)
        {
            iAccountRepository.SaveAccount(acc);
        }

        public void UpdateAccount(AccountMember acc)
        {
            iAccountRepository.UpdateAccount(acc);
        }

        public PaginatedAccountMembers SearchAccount(string fullName, string email, int roleId, int pageNum)
        {
            var dataList = iAccountRepository.SearchAccount(fullName, email, roleId);

            if (!dataList.Any())
            {
                return new PaginatedAccountMembers();
            }

            return PaginatedAccountMembers.ToPaginatedAccountMembers(dataList, pageNum);
        }
    }
}

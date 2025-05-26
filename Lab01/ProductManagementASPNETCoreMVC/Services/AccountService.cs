using BusinessObjects;
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
    }
}

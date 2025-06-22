using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _repo;
        public SystemAccountService(ISystemAccountRepository repo)
        {
            _repo = repo;
        }
        public async Task<SystemAccount> Login(string email, string password)
        {
            return await _repo.Login(email, password);
        }

        public Task<List<SystemAccount>> Get()
        {
            return _repo.Get();
        }

        public Task<SystemAccount> Get(int id)
        {
            return _repo.Get(id);
        }

        public Task<SystemAccount> Add(SystemAccount account)
        {
            return _repo.Add(account);
        }

        public Task<SystemAccount> Update(SystemAccount account)
        {
            return _repo.Update(account);
        }

        public Task<SystemAccount> Delete(int id)
        {
            return _repo.Delete(id);
        }
    }

}

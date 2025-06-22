using BusinessObjects.Models;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public async Task<SystemAccount> Login(string email, string password)
        {
            return await SystemAccountDAO.Instance.Login(email, password);
        }

        public Task<List<SystemAccount>> Get()
        {
            return SystemAccountDAO.Instance.GetAll();
        }

        public Task<SystemAccount> Get(int id)
        {
            return SystemAccountDAO.Instance.GetById(id);
        }

        public Task<SystemAccount> Add(SystemAccount account)
        {
            return SystemAccountDAO.Instance.Add(account);
        }

        public Task<SystemAccount> Update(SystemAccount account)
        {
            return SystemAccountDAO.Instance.Update(account);
        }

        public Task<SystemAccount> Delete(int id)
        {
            return SystemAccountDAO.Instance.Delete(id);
        }

    }

}

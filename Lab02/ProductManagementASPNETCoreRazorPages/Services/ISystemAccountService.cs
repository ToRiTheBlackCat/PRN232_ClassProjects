using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ISystemAccountService
    {
        Task<SystemAccount> Login(string email, string password);

        Task<List<SystemAccount>> Get();
        Task<SystemAccount> Get(int id);
        Task<SystemAccount> Add(SystemAccount category);
        Task<SystemAccount> Update(SystemAccount category);
        Task<SystemAccount> Delete(int id);
    }

}

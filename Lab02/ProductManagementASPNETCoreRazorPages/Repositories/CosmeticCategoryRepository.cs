using BusinessObjects.Models;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICosmeticCategoryRepository
    {
        Task<List<CosmeticCategory>> Get();
        Task<CosmeticCategory> Get(string id);
        Task<CosmeticCategory> Add(CosmeticCategory category);
        Task<CosmeticCategory> Update(CosmeticCategory category);
        Task<CosmeticCategory> Delete(string id);
    }

    public class CosmeticCategoryRepository : ICosmeticCategoryRepository
    {
        public Task<List<CosmeticCategory>> Get()
        {
            return CosmeticCategoryDAO.Instance.GetAll();
        }

        public Task<CosmeticCategory> Get(string id)
        {
            return CosmeticCategoryDAO.Instance.GetById(id);
        }

        public Task<CosmeticCategory> Add(CosmeticCategory category)
        {
            return CosmeticCategoryDAO.Instance.Add(category);
        }

        public Task<CosmeticCategory> Update(CosmeticCategory category)
        {
            return CosmeticCategoryDAO.Instance.Update(category);
        }

        public Task<CosmeticCategory> Delete(string id)
        {
            return CosmeticCategoryDAO.Instance.Delete(id);
        }
    }
}

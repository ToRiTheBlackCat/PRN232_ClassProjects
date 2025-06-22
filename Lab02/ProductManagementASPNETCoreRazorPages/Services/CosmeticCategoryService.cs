using BusinessObjects.Models;
using DataAccessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICosmeticCategoryService
    {
        Task<List<CosmeticCategory>> Get();
        Task<CosmeticCategory> Get(string id);
        Task<CosmeticCategory> Add(CosmeticCategory category);
        Task<CosmeticCategory> Update(CosmeticCategory category);
        Task<CosmeticCategory> Delete(string id);
    }

    public class CosmeticCategoryService : ICosmeticCategoryService
    {
        private readonly ICosmeticCategoryRepository _repo;
        public CosmeticCategoryService()
        {
            _repo = new CosmeticCategoryRepository();
        }

        public Task<List<CosmeticCategory>> Get()
        {
            return _repo.Get();
        }

        public Task<CosmeticCategory> Get(string id)
        {
            return _repo.Get(id);
        }

        public Task<CosmeticCategory> Add(CosmeticCategory category)
        {
            return _repo.Add(category);
        }

        public Task<CosmeticCategory> Update(CosmeticCategory category)
        {
            return _repo.Update(category);
        }

        public Task<CosmeticCategory> Delete(string id)
        {
            return _repo.Delete(id);
        }
    }
}

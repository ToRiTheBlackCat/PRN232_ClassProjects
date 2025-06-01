using FUNewsManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Service
{
    public class DashboardService
    {
        private readonly DashboardRepository _repo;

        public DashboardService(DashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> GetTotalNewsCount()
        {
            return await _repo.GetTotalNewsCount(null);
        }
        public async Task<int> GetTotalNewsCountByCategory(string cateName)
        {
            return await _repo.GetTotalNewsCount(cateName.Trim());
        }
    }
}

using FUNewsManagementSystem.Repository;
using FUNewsManagementSystem.Repository.Models.FormModels;
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
        public async Task<int> GetTotalNewsCountByDate(DateTime fromDate, DateTime toDate)
        {
            return await _repo.GetTotalNewsCountByDate(fromDate, toDate);
        }
        
    }
}

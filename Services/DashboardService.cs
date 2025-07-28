using BOs.Models;
using DAOs;
using Repos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepo _dashboardRepo;
        private readonly IAccountService _accountService;

        public DashboardService(IDashboardRepo dashboardRepo, IAccountService accountService)
        {
            _dashboardRepo = dashboardRepo;
            _accountService = accountService;
        }

        private async Task ValidateAccountAsync(int accountId)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null || !account.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Account is not active or not found.");
            }
        }

        public async Task<Dictionary<string, int>> GetOverviewAsync(int accountId)
        {
            await ValidateAccountAsync(accountId);
            return await _dashboardRepo.GetDashboardOverviewAsync();
        }

        public async Task<List<RecentActivityData>> GetRecentActivitiesAsync(int accountId, int page, int pageSize, string? type)
        {
            await ValidateAccountAsync(accountId);
            return await _dashboardRepo.GetRecentActivitiesAsync(page, pageSize, type);
        }

        public async Task<(Dictionary<string, int> byGender, Dictionary<string, int> byAge, List<(string className, int count)> byClass)> GetStudentDistributionAsync(int accountId)
        {
            await ValidateAccountAsync(accountId);
            return await _dashboardRepo.GetStudentDistributionAsync();
        }
    }
}
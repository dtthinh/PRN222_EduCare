using System.Collections.Generic;
using System.Threading.Tasks;
using BOs.Models;
using DAOs;

namespace Services
{
    public interface IDashboardService
    {
        Task<Dictionary<string, int>> GetOverviewAsync(int accountId);
        Task<List<RecentActivityData>> GetRecentActivitiesAsync(int accountId, int page, int pageSize, string? type);
        Task<(Dictionary<string, int> byGender, Dictionary<string, int> byAge, List<(string className, int count)> byClass)> GetStudentDistributionAsync(int accountId);
    }
}
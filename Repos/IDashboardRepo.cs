using System.Collections.Generic;
using System.Threading.Tasks;
using BOs.Models;
using DAOs;

namespace Repos
{
    public interface IDashboardRepo
    {
        Task<Dictionary<string, int>> GetDashboardOverviewAsync();
        Task<List<RecentActivityData>> GetRecentActivitiesAsync(int page, int pageSize, string? type);
        Task<(Dictionary<string, int> byGender, Dictionary<string, int> byAge, List<(string className, int count)> byClass)> GetStudentDistributionAsync();
    }
}
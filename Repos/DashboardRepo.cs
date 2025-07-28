using BOs.Models;
using DAOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repos
{
    public class DashboardRepo : IDashboardRepo
    {
        private readonly DashboardDAO _dashboardDAO;

        public DashboardRepo(DashboardDAO dashboardDAO)
        {
            _dashboardDAO = dashboardDAO;
        }

        public Task<Dictionary<string, int>> GetDashboardOverviewAsync()
        {
            return _dashboardDAO.GetDashboardOverviewAsync();
        }

        public Task<List<RecentActivityData>> GetRecentActivitiesAsync(int page, int pageSize, string? type)
        {
            return _dashboardDAO.GetRecentActivitiesAsync(page, pageSize, type);
        }

        public Task<(Dictionary<string, int> byGender, Dictionary<string, int> byAge, List<(string className, int count)> byClass)> GetStudentDistributionAsync()
        {
            return _dashboardDAO.GetStudentDistributionAsync();
        }
    }
}
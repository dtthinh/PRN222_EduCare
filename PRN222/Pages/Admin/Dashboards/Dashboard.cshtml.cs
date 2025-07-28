using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using DAOs;
using System.Linq;
using BOs.Models;

public class DashboardModel : PageModel
{
    private readonly IDashboardService _dashboardService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardModel(IDashboardService dashboardService, IHttpContextAccessor httpContextAccessor)
    {
        _dashboardService = dashboardService;
        _httpContextAccessor = httpContextAccessor;
    }

    public Dictionary<string, int> OverviewData { get; set; } = new Dictionary<string, int>();
    public List<RecentActivityData> RecentActivities { get; set; } = new List<RecentActivityData>();
    public (Dictionary<string, int> byGender, Dictionary<string, int> byAge, List<(string className, int count)> byClass) StudentDistribution { get; set; }
    public (List<(string Name, int Used, int Remaining)> MostUsedSupplies, List<(string Name, int Current, int Minimum)> LowStockAlerts, List<(string Name, string ExpiryDate, int Quantity)> ExpiringItems) SupplyUsage { get; set; }
    public (double OverallCoverage, List<(string CampaignName, int TargetCount, int CompletedCount, double CoverageRate, double ConsentRate)> ByCampaign, List<(string ClassName, double Coverage)> ByClass) VaccinationCoverage { get; set; }

    public string Period { get; set; }
    public string StudentGenderChartDataJson { get; set; }
    public string StudentAgeChartDataJson { get; set; }
    public string MedicalEventsTimelineJson { get; set; }

    public async Task<IActionResult> OnGetAsync(string period = "30d")
    {
        Period = period;
        var accountId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");

        if (accountId == null)
        {
            return RedirectToPage("/Credential/Login");
        }

        OverviewData = await _dashboardService.GetOverviewAsync(accountId.Value);
        RecentActivities = await _dashboardService.GetRecentActivitiesAsync(accountId.Value, 1, 5, null);
        StudentDistribution = await _dashboardService.GetStudentDistributionAsync(accountId.Value);

        StudentGenderChartDataJson = JsonSerializer.Serialize(new
        {
            labels = StudentDistribution.byGender.Keys.ToList(),
            datasets = new[] { new { data = StudentDistribution.byGender.Values.ToList() } }
        });

        StudentAgeChartDataJson = JsonSerializer.Serialize(new
        {
            labels = StudentDistribution.byAge.Keys.ToList(),
            datasets = new[] { new { data = StudentDistribution.byAge.Values.ToList() } }
        });

        return Page();
    }
}
using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.Threading.Tasks;

namespace PRN222.Pages.Nurse.Health_Check
{
    public class HealthCheckModel : PageModel
    {
        private IHealthCheckService _healthCheckService;

        public HealthCheckModel(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        public List<HealthCheck> HealthChecks { get; set; } = new();

        public async Task OnGetAsync()
        {
            HealthChecks = await _healthCheckService.GetAllHealthChecksAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services; // Namespace của IHealthCheckService
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN222.Pages.Parent
{
    public class HealthCheckModel : PageModel
    {
        private readonly IHealthCheckService _healthCheckService;

        public HealthCheckModel(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        // Thuộc tính để lưu trữ danh sách các lần khám sức khỏe
        public IList<HealthCheck> HealthChecks { get; set; } = new List<HealthCheck>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return BadRequest("Parent ID is required.");
            }

            HealthChecks = await _healthCheckService.GetHealthChecksByParentIdAsync(id);

            return Page();
        }
    }
}
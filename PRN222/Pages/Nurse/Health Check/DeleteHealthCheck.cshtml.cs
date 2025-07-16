using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace PRN222.Pages.Nurse.Health_Check
{
    public class DeleteHealthCheckModel : PageModel
    {
        private readonly IHealthCheckService _healthCheckService;

        public DeleteHealthCheckModel(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [BindProperty]
        public HealthCheck HealthCheck { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var allChecks = await _healthCheckService.GetAllHealthChecksAsync();
            HealthCheck = allChecks.FirstOrDefault(h => h.HealthCheckID == id);

            if (HealthCheck == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _healthCheckService.DeleteHealthCheckAsync(HealthCheck.HealthCheckID);
            return RedirectToPage("HealthCheck");
        }
    }
}

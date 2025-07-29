using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Model;
using Services;

namespace PRN222.Pages.Nurse.Health_Check
{
    public class EditHealthCheckModel : PageModel
    {
        private readonly IHealthCheckService _healthCheckService;

        public EditHealthCheckModel(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [BindProperty]
        public HealthCheckUpdateModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var check = await _healthCheckService.GetHealthCheckByIdAsync(id);

            if (check == null)
                return NotFound();

            Input = new HealthCheckUpdateModel
            {
                HealthCheckID = check.HealthCheckID,
                Result = check.Result ?? "",
                Height = check.Height ?? 0,
                Weight = check.Weight ?? 0,
                LeftEye = check.LeftEye ?? 0,
                RightEye = check.RightEye ?? 0
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var healthCheckUpdated = await _healthCheckService.GetHealthCheckByIdAsync(Input.HealthCheckID);

            healthCheckUpdated.Result = Input.Result;
            healthCheckUpdated.Height = Input.Height;
            healthCheckUpdated.Weight = Input.Weight;
            healthCheckUpdated.LeftEye = Input.LeftEye;
            healthCheckUpdated.RightEye = Input.RightEye;


            await _healthCheckService.UpdateHealthCheckAsync(healthCheckUpdated);
            return RedirectToPage("HealthCheck");
        }
    }
}

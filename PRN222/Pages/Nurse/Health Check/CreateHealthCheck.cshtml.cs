using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Model;
using Services;

namespace PRN222.Pages.Nurse.Health_Check
{
    public class CreateHealthCheckModel : PageModel
    {
        private readonly IHealthCheckService _healthCheckService;
        private readonly IClassService _classService;
        private readonly IStudentService _studentService;

        public CreateHealthCheckModel(IHealthCheckService healthCheckService, IClassService classService, IStudentService studentService)
        {
            _healthCheckService = healthCheckService;
            _classService = classService;
            _studentService = studentService;
        }

        [BindProperty]
        public HealthCheckBatchCreate Input { get; set; } = new();

        public List<Class> AllClasses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            AllClasses = await _classService.GetAllClassesAsync();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            Input.NurseId = userId.Value;
            Input.Date = DateTime.Now;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            Input.NurseId = userId.Value;

            if (!ModelState.IsValid || Input.ClassIds == null || !Input.ClassIds.Any())
            {
                AllClasses = await _classService.GetAllClassesAsync();
                ModelState.AddModelError("", "Please select at least one class.");
                return Page();
            }

            var students = await _studentService.GetStudentsByClassIdsAsync(Input.ClassIds);

            if (!students.Any())
            {
                ModelState.AddModelError("", "No students found for the selected classes.");
                AllClasses = await _classService.GetAllClassesAsync();
                return Page();
            }

            foreach (var student in students)
            {

                var healthCheck = new HealthCheck
                {
                    NurseID = Input.NurseId,
                    StudentID = student.StudentId,
                    ParentID = student.ParentId,
                    Date = Input.Date,
                    HealthCheckDescription = Input.HealthCheckDescription,
                };
                var created = await _healthCheckService.CreateHealthCheckAsync(healthCheck);

            }

            TempData["SuccessMessage"] = "Health checks created successfully.";
            return RedirectToPage("HealthCheck");
        }

    }
}

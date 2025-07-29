using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System;
using System.Threading.Tasks;

namespace PRN222.Pages.Nurse.VaccinationFollowUp
{
    public class CreateFollowUpModel : PageModel
    {
        private readonly IVaccinationService _vaccinationService;

        public CreateFollowUpModel(IVaccinationService vaccinationService)
        {
            _vaccinationService = vaccinationService;
        }

        [BindProperty]
        public BOs.Models.VaccinationFollowUp FollowUp { get; set; } = new BOs.Models.VaccinationFollowUp();
        public int RecordId { get; set; }
        public string? Message { get; set; }

        public void OnGet(int recordId)
        {
            RecordId = recordId;
            FollowUp.RecordId = recordId;
            FollowUp.Date = DateTime.Today;
        }

        public async Task<IActionResult> OnPostAsync(int recordId)
        {
            RecordId = recordId;
            FollowUp.RecordId = recordId;
            ModelState.Remove("FollowUp.Record");
            if (!ModelState.IsValid)
            {
                Message = "Please correct the errors in the form.";
                return Page();
            }
            await _vaccinationService.CreateFollowUpAsync(FollowUp);
            Message = "Follow up created successfully.";
            return RedirectToPage("/Nurse/VaccinationFollowUp/FollowUp", new { recordId });
        }
    }
}

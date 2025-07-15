using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace PRN222.Pages.Nurse
{
    public class MedicationRequestModel : PageModel
    {
        private readonly IParentMedicationRequestService _parentMedicationService;

        public MedicationRequestModel(IParentMedicationRequestService parentMedicationService)
        {
            _parentMedicationService = parentMedicationService;
        }

        public List<ParentMedicationRequest> Medications { get; set; } = new();

        public async Task OnGetAsync()
        {
            Medications = await _parentMedicationService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id, string nurseNote)
        {
            var request = await _parentMedicationService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Approved";
            request.NurseNote = nurseNote;
            await _parentMedicationService.UpdateAsync(request);

            return RedirectToPage(); // Refresh page to show updates
        }

        public async Task<IActionResult> OnPostRejectAsync(int id, string nurseNote)
        {
            var request = await _parentMedicationService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Rejected";
            request.NurseNote = nurseNote;
            await _parentMedicationService.UpdateAsync(request);

            return RedirectToPage(); // Refresh page to show updates
        }
    }
}

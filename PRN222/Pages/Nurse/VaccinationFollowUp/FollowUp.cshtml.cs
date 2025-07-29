using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using BOs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN222.Pages.Nurse.VaccinationFollowUp
{
    public class FollowUpModel : PageModel
    {
        private readonly IVaccinationService _vaccinationService;

        public FollowUpModel(IVaccinationService vaccinationService)
        {
            _vaccinationService = vaccinationService;
        }

        public List<BOs.Models.VaccinationFollowUp> FollowUps { get; set; } = new();
        public int RecordId { get; set; }

        public async Task OnGetAsync(int recordId)
        {
            RecordId = recordId;
            FollowUps = await _vaccinationService.GetFollowUpsByRecordAsync(recordId);
        }
    }
}

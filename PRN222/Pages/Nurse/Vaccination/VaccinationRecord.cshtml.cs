using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using BOs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN222.Pages.Nurse.Vaccination
{
    public class VaccinationRecordModel : PageModel
    {
        private readonly IVaccinationService _vaccinationService;

        public VaccinationRecordModel(IVaccinationService vaccinationService)
        {
            _vaccinationService = vaccinationService;
        }

        public List<VaccinationRecord> Records { get; set; } = new();

        public async Task OnGetAsync()
        {
            Records = await _vaccinationService.GetAllRecordsAsync();
        }
    }
}

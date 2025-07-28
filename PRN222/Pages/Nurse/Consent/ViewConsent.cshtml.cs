using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN222.Pages.Nurse.Consent
{
    public class ViewConsentModel : PageModel
    {
        private readonly IVaccinationService _vaccinationService;

        public ViewConsentModel(IVaccinationService vaccinationService)
        {
            _vaccinationService = vaccinationService;
        }

        public List<VaccinationConsent> VaccinationConsents { get; set; } = new List<VaccinationConsent>();

        public async Task OnGetAsync()
        {
            VaccinationConsents = await _vaccinationService.GetAllConsentsAsync();
        }
    }
}

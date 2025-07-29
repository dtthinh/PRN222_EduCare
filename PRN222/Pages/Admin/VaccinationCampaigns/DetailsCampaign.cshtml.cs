using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class DetailsCampaignModel : PageModel
{
    private readonly IVaccinationService _vaccinationService;

    public DetailsCampaignModel(IVaccinationService vaccinationService)
    {
        _vaccinationService = vaccinationService;
    }

    public VaccinationCampaign Campaign { get; set; }

    public List<VaccinationConsent> Consents { get; set; }
    public List<VaccinationConsent> ApprovedConsents { get; set; }
    public List<VaccinationConsent> RejectedConsents { get; set; }
    public List<VaccinationConsent> PendingConsents { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Campaign = await _vaccinationService.GetCampaignByIdAsync(id.Value);
        Consents = await _vaccinationService.GetConsentsByCampaignAsync(id.Value);

        if (Campaign == null)
        {
            return NotFound();
        }

        ApprovedConsents = Consents.Where(c => c.IsAgreed == true).ToList();
        RejectedConsents = Consents.Where(c => c.IsAgreed == false).ToList();
        PendingConsents = Consents.Where(c => c.IsAgreed == null).ToList();

        return Page();
    }
}
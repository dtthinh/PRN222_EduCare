using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using BOs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CampaignsModel : PageModel
{
    private readonly IVaccinationService _vaccinationService;

    public CampaignsModel(IVaccinationService vaccinationService)
    {
        _vaccinationService = vaccinationService;
    }

    public List<VaccinationCampaign> Campaigns { get; set; } = new List<VaccinationCampaign>();

    public async Task OnGetAsync()
    {
        Campaigns = await _vaccinationService.GetAllCampaignsAsync() ?? new List<VaccinationCampaign>();
    }
}
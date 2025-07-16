using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using BOs.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CreateCampaignModel : PageModel
{
    private readonly IVaccinationService _vaccinationService;

    public CreateCampaignModel(IVaccinationService vaccinationService)
    {
        _vaccinationService = vaccinationService;
    }

    [BindProperty]
    public VaccinationCampaign Campaign { get; set; } = new();

    public List<SelectListItem> VaccineOptions { get; set; } = new();

    public async Task OnGetAsync()
    {
        var vaccines = await _vaccinationService.GetAllVaccinesAsync();
        VaccineOptions = vaccines.Select(v => new SelectListItem
        {
            Value = v.VaccineId.ToString(),
            Text = v.Name
        }).ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"❌ {key}: {error.ErrorMessage}");
                }
            }

            await OnGetAsync(); // để reload dropdown
            return Page();
        }

        await _vaccinationService.CreateCampaignAsync(Campaign);
        return RedirectToPage("Campaigns");
    }
}

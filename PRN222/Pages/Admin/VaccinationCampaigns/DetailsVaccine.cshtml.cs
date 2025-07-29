using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System.Threading.Tasks;

public class DetailsVaccineModel : PageModel
{
    private readonly IVaccinationService _vaccinationService;

    public DetailsVaccineModel(IVaccinationService vaccinationService)
    {
        _vaccinationService = vaccinationService;
    }

    public Vaccine Vaccine { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Vaccine = await _vaccinationService.GetVaccineByIdAsync(id.Value);

        if (Vaccine == null)
        {
            return NotFound();
        }

        return Page();
    }
}
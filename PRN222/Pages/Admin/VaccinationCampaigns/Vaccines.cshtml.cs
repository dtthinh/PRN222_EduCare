using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using BOs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class VaccinesModel : PageModel
{
    private readonly IVaccinationService _vaccinationService;

    public VaccinesModel(IVaccinationService vaccinationService)
    {
        _vaccinationService = vaccinationService;
    }

    public List<Vaccine> Vaccines { get; set; }

    public async Task OnGetAsync()
    {
        Vaccines = await _vaccinationService.GetAllVaccinesAsync();
    }
}

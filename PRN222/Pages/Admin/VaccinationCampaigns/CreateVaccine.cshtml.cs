using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System.Threading.Tasks;

public class CreateVaccineModel : PageModel
{
    private readonly IVaccinationService _vaccinationService;

    public CreateVaccineModel(IVaccinationService vaccinationService)
    {
        _vaccinationService = vaccinationService;
    }

    [BindProperty]
    public Vaccine Vaccine { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _vaccinationService.CreateVaccineAsync(Vaccine);

        TempData["SuccessMessage"] = "Tạo vaccine mới thành công!";
        return RedirectToPage("Vaccines");
    }
}
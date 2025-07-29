using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System.Threading.Tasks;

public class DetailsModel : PageModel
{
    private readonly IMedicalEventService _service;

    public DetailsModel(IMedicalEventService service)
    {
        _service = service;
    }

    public MedicalEvent MedicalEvent { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        MedicalEvent = await _service.GetMedicalEventByIdAsync(id.Value);

        if (MedicalEvent == null)
        {
            return NotFound();
        }
        return Page();
    }
}
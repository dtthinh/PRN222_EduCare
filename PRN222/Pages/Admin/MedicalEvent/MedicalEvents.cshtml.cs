using BOs.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Services;

public class MedicalEventsModel : PageModel
{
    private readonly IMedicalEventService _service;

    public MedicalEventsModel(IMedicalEventService service)
    {
        _service = service;
    }

    public List<MedicalEvent> MedicalEvents { get; set; }

    public async Task OnGetAsync()
    {
        MedicalEvents = await _service.GetAllMedicalEventsAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _service.DeleteMedicalEventAsync(id);
        return RedirectToPage();
    }
}

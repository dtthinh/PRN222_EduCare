using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System.Threading.Tasks;

public class DeleteModel : PageModel
{
    private readonly IMedicalEventService _service;

    public DeleteModel(IMedicalEventService service)
    {
        _service = service;
    }

    [BindProperty]
    public MedicalEvent MedicalEvent { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        MedicalEvent = await _service.GetMedicalEventByIdAsync(id.Value);
        if (MedicalEvent == null) return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();

        await _service.DeleteMedicalEventAsync(id.Value);

        TempData["SuccessMessage"] = "Đã xóa sự kiện thành công.";
        return RedirectToPage("./MedicalEvents");
    }
}
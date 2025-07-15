using BOs.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Repos;
using Services;

public class CreateMedicalEventModel(IMedicalEventService service, IAccountService accountService, IStudentRepo studentRepo) : PageModel
{
    private readonly IMedicalEventService _service = service;
    private readonly IAccountService _accountService = accountService;
    private readonly IStudentRepo _studentRepo = studentRepo;

    [BindProperty]
    public MedicalEvent MedicalEvent { get; set; }

    public SelectList StudentsSelectList { get; set; }
    public SelectList NursesSelectList { get; set; }

    public async Task OnGetAsync()
    {
        var students = await _studentRepo.GetAllStudentsAsync();
        var nurses = await _accountService.GetActiveNursesAsync();
        StudentsSelectList = new SelectList(students, "StudentId", "Fullname");
        NursesSelectList = new SelectList(nurses, "AccountID", "Fullname");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var result = await _service.CreateMedicalEventAsync(MedicalEvent, new(), new());
        return RedirectToPage("MedicalEvents");
    }
}

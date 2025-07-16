using BOs.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repos;
using Services;

public class DashboardModel : PageModel
{
    private readonly IAccountService _accountService;
    private readonly IMedicalEventService _medicalEventService;
    private readonly IStudentRepo _studentRepo;

    public DashboardModel(
        IAccountService accountService,
        IMedicalEventService medicalEventService,
        IStudentRepo studentRepo)
    {
        _accountService = accountService;
        _medicalEventService = medicalEventService;
        _studentRepo = studentRepo;
    }

    public int StudentCount { get; set; }
    public int ParentCount { get; set; }
    public int NurseCount { get; set; }
    public int MedicalEventCount { get; set; }

    public async Task OnGetAsync()
    {
        var students = await _studentRepo.GetAllStudentsAsync();
        StudentCount = students.Count;

        ParentCount = await _accountService.GetParentCountAsync();
        NurseCount = await _accountService.GetNurseCountAsync();

        var events = await _medicalEventService.GetAllMedicalEventsAsync();
        MedicalEventCount = events.Count;
    }
}

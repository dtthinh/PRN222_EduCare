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
    private readonly IStudentService _studentService;
    private readonly IClassService _classService;

    public CreateCampaignModel(IVaccinationService vaccinationService, IStudentService studentService, IClassService classService)
    {
        _vaccinationService = vaccinationService;
        _studentService = studentService;
        _classService = classService;
    }

    [BindProperty]
    public VaccinationCampaign Campaign { get; set; } = new();

    [BindProperty]
    public List<int> SelectedClassIds { get; set; } = new();

    public List<SelectListItem> VaccineOptions { get; set; } = new();
    public List<SelectListItem> ClassOptions { get; set; } = new();

    public async Task OnGetAsync()
    {
        var vaccines = await _vaccinationService.GetAllVaccinesAsync();
        var classes = await _classService.GetAllClassesAsync();

        VaccineOptions = vaccines.Select(v => new SelectListItem
        {
            Value = v.VaccineId.ToString(),
            Text = v.Name
        }).ToList();

        ClassOptions = classes.Select(c => new SelectListItem
        {
            Value = c.ClassId.ToString(),
            Text = c.ClassName
        }).ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(); // Reload dropdowns
            return Page();
        }

        // Check duplicate campaign name
        if (await _vaccinationService.CampaignNameExistsAsync(Campaign.Name))
        {
            ModelState.AddModelError("Campaign.Name", "Tên chiến dịch đã tồn tại.");
            await OnGetAsync();
            return Page();
        }

        // Check time conflict
        if (await _vaccinationService.CampaignTimeConflictAsync(Campaign.Date))
        {
            ModelState.AddModelError("Campaign.Date", "Đã có chiến dịch khác trong vòng 30 phút.");
            await OnGetAsync();
            return Page();
        }

        // Create campaign
        var createdCampaign = await _vaccinationService.CreateCampaignAsync(Campaign);

        // Get students by selected classes
        var students = await _studentService.GetStudentsByClassIdsAsync(SelectedClassIds);
        foreach (var student in students)
        {
            if (student?.ParentId != null)
            {
                var consent = new VaccinationConsent
                {
                    CampaignId = createdCampaign.CampaignId,
                    StudentId = student.StudentId,
                    ParentId = student.ParentId.Value,
                    IsAgreed = null,
                    Note = null,
                    DateConfirmed = null
                };
                await _vaccinationService.CreateConsentAsync(consent);
            }
        }

        return RedirectToPage("Campaigns");
    }
}

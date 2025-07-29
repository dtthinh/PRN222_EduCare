using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using BOs.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

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

    private async Task PopulateSelectLists()
    {
        var vaccines = await _vaccinationService.GetAllVaccinesAsync() ?? new List<Vaccine>();
        var classes = await _classService.GetAllClassesAsync() ?? new List<Class>();

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

    public async Task OnGetAsync()
    {
        Campaign.Date = DateTime.Today;
        await PopulateSelectLists();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectLists();
            return Page();
        }

        if (await _vaccinationService.CampaignNameExistsAsync(Campaign.Name))
        {
            ModelState.AddModelError("Campaign.Name", "Tên chiến dịch này đã tồn tại. Vui lòng chọn một tên khác.");
            await PopulateSelectLists();
            return Page();
        }

        if (await _vaccinationService.CampaignTimeConflictAsync(Campaign.Date))
        {
            ModelState.AddModelError("Campaign.Date", "Đã có một chiến dịch khác được lên lịch trong vòng 30 phút so với thời gian bạn chọn.");
            await PopulateSelectLists();
            return Page();
        }

        if (SelectedClassIds == null || !SelectedClassIds.Any())
        {
            ModelState.AddModelError("SelectedClassIds", "Vui lòng chọn ít nhất một lớp học để áp dụng chiến dịch.");
            await PopulateSelectLists();
            return Page();
        }

        var createdCampaign = await _vaccinationService.CreateCampaignAsync(Campaign);

        var students = await _studentService.GetStudentsByClassIdsAsync(SelectedClassIds);
        foreach (var student in students.Where(s => s?.ParentId != null))
        {
            var consent = new VaccinationConsent
            {
                CampaignId = createdCampaign.CampaignId,
                StudentId = student.StudentId,
                ParentId = student.ParentId.Value
            };
            await _vaccinationService.CreateConsentAsync(consent);
        }

        TempData["SuccessMessage"] = "Tạo chiến dịch tiêm chủng thành công!";
        return RedirectToPage("Campaigns");
    }
}
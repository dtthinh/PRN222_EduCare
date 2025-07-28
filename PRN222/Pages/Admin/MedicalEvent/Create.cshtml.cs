using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;

public class MedicationUsageDto
{
    public int MedicationId { get; set; }
    public int QuantityUsed { get; set; }
}
public class MedicalSupplyUsageDto
{
    public int MedicalSupplyId { get; set; }
    public int QuantityUsed { get; set; }
}

public class CreateModel : PageModel
{
    private readonly IMedicalEventService _eventService;
    private readonly IMedicationService _medicationService;
    private readonly IMedicalSupplyService _supplyService;
    private readonly IClassService _classService;
    private readonly IStudentService _studentService;
    private readonly IAccountService _accountService;

    public CreateModel(IMedicalEventService eventService, IMedicationService medicationService, IMedicalSupplyService supplyService, IClassService classService, IStudentService studentService, IAccountService accountService)
    {
        _eventService = eventService;
        _medicationService = medicationService;
        _supplyService = supplyService;
        _classService = classService;
        _studentService = studentService;
        _accountService = accountService;
    }

    [BindProperty]
    public MedicalEvent MedicalEvent { get; set; } = new() { Description = "", Type = "" };

    [BindProperty]
    public List<MedicationUsageDto> SelectedMedications { get; set; } = new();

    [BindProperty]
    public List<MedicalSupplyUsageDto> SelectedSupplies { get; set; } = new();

    public IEnumerable<SelectListItem> ClassOptions { get; set; }
    public IEnumerable<SelectListItem> NurseOptions { get; set; }
    public IEnumerable<SelectListItem> EventTypeOptions { get; set; }
    public List<Medication> AllMedications { get; set; } = new();
    public List<MedicalSupply> AllSupplies { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        MedicalEvent.Date = DateTime.Today;
        await LoadInitialData();
        return Page();
    }

    public async Task<JsonResult> OnGetStudentsByClass(int classId)
    {
        var students = await _studentService.GetStudentsByClassIdAsync(classId);

        if (students == null)
        {
            return new JsonResult(new List<object>());
        }

        var result = students.Select(s => new { id = s.StudentId, name = s.Fullname });
        return new JsonResult(result);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("MedicalEvent.StudentId");
        ModelState.Clear();
        if (!ModelState.IsValid)
        {
            await LoadInitialData();
            return Page();
        }

        if (MedicalEvent.StudentId == 0)
        {
            ModelState.AddModelError("", "Vui lòng chọn lớp và học sinh.");
            await LoadInitialData();
            return Page();
        }

        var medicationUsages = SelectedMedications.Where(m => m.MedicationId > 0 && m.QuantityUsed > 0).Select(m => new MedicalEventMedication { MedicationId = m.MedicationId, QuantityUsed = m.QuantityUsed }).ToList();
        var supplyUsages = SelectedSupplies.Where(s => s.MedicalSupplyId > 0 && s.QuantityUsed > 0).Select(s => new MedicalEventMedicalSupply { MedicalSupplyId = s.MedicalSupplyId, QuantityUsed = s.QuantityUsed }).ToList();

        await _eventService.CreateMedicalEventAsync(MedicalEvent, medicationUsages, supplyUsages);

        TempData["SuccessMessage"] = $"Tạo sự kiện thành công!";
        return RedirectToPage("./MedicalEvents");
    }

    private async Task LoadInitialData()
    {
        var classesFromDb = await _classService.GetAllClassesAsync();
        var nursesFromDb = await _accountService.GetActiveNursesAsync();
        AllMedications = await _medicationService.GetAllAsync() ?? new List<Medication>();
        AllSupplies = await _supplyService.GetAllAsync() ?? new List<MedicalSupply>();

        ClassOptions = classesFromDb?.Where(c => c != null).Select(c => new SelectListItem { Value = c.ClassId.ToString(), Text = c.ClassName }).ToList() ?? new List<SelectListItem>();
        NurseOptions = nursesFromDb?.Where(n => n != null).Select(n => new SelectListItem { Value = n.AccountID.ToString(), Text = n.Fullname }).ToList() ?? new List<SelectListItem>();

        var eventTypes = new List<string> { "Tai nạn", "Bệnh tật", "Sốt", "Khác" };
        EventTypeOptions = new SelectList(eventTypes);
    }
}
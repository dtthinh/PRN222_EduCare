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
        _eventService = eventService; _medicationService = medicationService; _supplyService = supplyService; _classService = classService; _studentService = studentService; _accountService = accountService;
    }

    [BindProperty] public MedicalEvent MedicalEvent { get; set; } = new() { Description = "", Type = "" };

    [BindProperty]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn một lớp học.")]
    public int SelectedClassId { get; set; }

    [BindProperty] public List<Medication> SelectedMedications { get; set; } = new();
    [BindProperty] public List<MedicalSupply> SelectedSupplies { get; set; } = new();

    public IEnumerable<SelectListItem> ClassOptions { get; set; }
    public IEnumerable<SelectListItem> NurseOptions { get; set; }
    public List<Medication> AllMedications { get; set; } = new();
    public List<MedicalSupply> AllSupplies { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        MedicalEvent.Date = DateTime.Today;
        await LoadInitialData();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadInitialData();
            return Page();
        }

        var studentsInClass = await _studentService.GetStudentsByClassIdAsync(SelectedClassId);
        if (studentsInClass == null || !studentsInClass.Any())
        {
            ModelState.AddModelError(string.Empty, "Lớp học được chọn không có học sinh nào.");
            await LoadInitialData();
            return Page();
        }

        var medicationUsages = SelectedMedications.Where(m => m.MedicationId > 0 && m.Quantity > 0).Select(m => new MedicalEventMedication { MedicationId = m.MedicationId, QuantityUsed = m.Quantity }).ToList();
        var supplyUsages = SelectedSupplies.Where(s => s.MedicalSupplyId > 0 && s.Quantity > 0).Select(s => new MedicalEventMedicalSupply { MedicalSupplyId = s.MedicalSupplyId, QuantityUsed = s.Quantity }).ToList();

        foreach (var student in studentsInClass.Where(s => s != null))
        {
            var newEvent = new MedicalEvent
            {
                StudentId = student.StudentId,
                NurseId = MedicalEvent.NurseId,
                Date = MedicalEvent.Date,
                Type = MedicalEvent.Type, // Lấy từ form
                Description = MedicalEvent.Description, // Lấy từ form
                Note = MedicalEvent.Note ?? ""
            };
            await _eventService.CreateMedicalEventAsync(newEvent, medicationUsages, supplyUsages);
        }

        TempData["SuccessMessage"] = $"Tạo sự kiện khám sức khỏe thành công cho {studentsInClass.Count()} học sinh!";
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
    }
}
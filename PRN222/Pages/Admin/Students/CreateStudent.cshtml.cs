using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;
using Services;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

public class CreateStudentModel : PageModel
{
    private readonly IStudentService _studentService;
    private readonly IClassService _classService;

    public CreateStudentModel(IStudentService studentService, IClassService classService)
    {
        _studentService = studentService;
        _classService = classService;
    }

    [BindProperty]
    public Student Student { get; set; } = new();

    public SelectList ClassOptions { get; set; }

    private async Task PopulateSelectLists()
    {
        var classes = await _classService.GetAllClassesAsync() ?? new List<Class>();
        ClassOptions = new SelectList(classes, "ClassId", "ClassName");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await PopulateSelectLists();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Clear();
        if (!ModelState.IsValid)
        {
            await PopulateSelectLists();
            return Page();
        }

        Student.CreatedAt = DateTime.UtcNow;
        Student.UpdateAt = DateTime.UtcNow;

        await _studentService.CreateStudentAsync(Student);

        TempData["SuccessMessage"] = "Thêm học sinh mới thành công!";
        return RedirectToPage("Students");
    }
}
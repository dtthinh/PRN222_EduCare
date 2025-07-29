using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;
using Services;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

public class EditStudentModel : PageModel
{
    private readonly IStudentService _studentService;
    private readonly IClassService _classService;

    public EditStudentModel(IStudentService studentService, IClassService classService)
    {
        _studentService = studentService;
        _classService = classService;
    }

    [BindProperty]
    public Student Student { get; set; }

    public SelectList ClassOptions { get; set; }

    private async Task PopulateSelectLists()
    {
        var classes = await _classService.GetAllClassesAsync() ?? new List<Class>();
        ClassOptions = new SelectList(classes, "ClassId", "ClassName", Student?.ClassId);
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        Student = await _studentService.GetStudentByIdAsync(id.Value);
        if (Student == null) return NotFound();

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

        var studentFromDb = await _studentService.GetStudentByIdAsync(Student.StudentId);
        if (studentFromDb == null) return NotFound();

        // Cập nhật các trường cho phép, giữ nguyên ParentId
        studentFromDb.Fullname = Student.Fullname;
        studentFromDb.StudentCode = Student.StudentCode;
        studentFromDb.DateOfBirth = Student.DateOfBirth;
        studentFromDb.Gender = Student.Gender;
        studentFromDb.ClassId = Student.ClassId;
        studentFromDb.UpdateAt = DateTime.UtcNow;

        await _studentService.UpdateStudentAsync(studentFromDb);

        TempData["SuccessMessage"] = "Cập nhật thông tin học sinh thành công!";
        return RedirectToPage("Students");
    }
}
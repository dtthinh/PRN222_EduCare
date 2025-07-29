using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System.Threading.Tasks;

public class DetailsStudentModel : PageModel
{
    private readonly IStudentService _studentService;

    public DetailsStudentModel(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public Student Student { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        Student = await _studentService.GetStudentByIdAsync(id.Value);
        if (Student == null) return NotFound();
        return Page();
    }
}
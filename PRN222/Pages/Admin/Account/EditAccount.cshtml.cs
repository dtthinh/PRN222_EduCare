using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class EditAccountModel : PageModel
{
    private readonly IAccountService _accountService;
    private readonly IStudentService _studentService;
    private readonly IClassService _classService;

    public EditAccountModel(IAccountService accountService, IStudentService studentService, IClassService classService)
    {
        _accountService = accountService;
        _studentService = studentService;
        _classService = classService;
    }

    [BindProperty]
    public Account Account { get; set; }

    [BindProperty]
    public int? NewStudentId { get; set; }

    public SelectList StatusOptions { get; set; }
    public SelectList ClassOptions { get; set; }
    public List<Student> LinkedStudents { get; set; } = new();

    private async Task PopulateSelectLists()
    {
        var statuses = new List<string> { "Active", "Inactive", "Pending", "Reject" };
        StatusOptions = new SelectList(statuses, Account?.Status);

        var classes = await _classService.GetAllClassesAsync() ?? new List<Class>();
        ClassOptions = new SelectList(classes, "ClassId", "ClassName");
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        Account = await _accountService.GetAccountByIdAsync(id.Value);
        if (Account == null) return NotFound();

        if (Account.RoleID == 3)
        {
            LinkedStudents = await _studentService.GetStudentsByParentIdAsync(id.Value);
        }

        await PopulateSelectLists();
        return Page();
    }

    public async Task<JsonResult> OnGetStudentsWithoutParentsByClass(int classId)
    {
        var students = await _studentService.GetStudentsWithoutParentsByClassIdAsync(classId);
        var result = students.Select(s => new { id = s.StudentId, name = $"{s.Fullname} ({s.StudentCode})" });
        return new JsonResult(result);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Clear();
        if (!ModelState.IsValid)
        {
            await PopulateSelectLists();
            // Tải lại danh sách học sinh nếu có lỗi
            if (Account.RoleID == 3)
            {
                LinkedStudents = await _studentService.GetStudentsByParentIdAsync(Account.AccountID);
            }
            return Page();
        }

        var accountToUpdate = await _accountService.GetAccountByIdAsync(Account.AccountID);
        if (accountToUpdate == null) return NotFound();

        accountToUpdate.Fullname = Account.Fullname;
        accountToUpdate.Status = Account.Status;
        accountToUpdate.UpdateAt = System.DateTime.UtcNow;

        await _accountService.UpdateAccountAsync(accountToUpdate);

        if (accountToUpdate.RoleID == 3 && NewStudentId.HasValue)
        {
            await _studentService.UpdateParentForStudentAsync(NewStudentId.Value, accountToUpdate.AccountID);
        }

        TempData["SuccessMessage"] = "Cập nhật tài khoản thành công!";
        return RedirectToPage("Accounts");
    }

    public async Task<IActionResult> OnPostRemoveStudentAsync(int studentId)
    {
        try
        {
            await _studentService.RemoveParentFromStudentAsync(studentId);
            return new JsonResult(new { success = true, message = "Đã loại bỏ học sinh thành công." });
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }
}

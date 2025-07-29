using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

public class CreateAccountModel : PageModel
{
    private readonly IAccountService _accountService;
    private readonly IClassService _classService;
    private readonly IStudentService _studentService;

    public CreateAccountModel(IAccountService accountService, IClassService classService, IStudentService studentService)
    {
        _accountService = accountService;
        _classService = classService;
        _studentService = studentService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    [BindProperty]
    public IFormFile ImageFile { get; set; }

    public SelectList RoleOptions { get; set; }
    public SelectList ClassOptions { get; set; }

    public class InputModel
    {
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        public string Fullname { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Vai trò")]
        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public int RoleID { get; set; }

        public int? SelectedStudentId { get; set; }
    }

    private async Task PopulateSelectLists()
    {
        var roles = await _accountService.GetAllRolesAsync() ?? new List<Role>();
        RoleOptions = new SelectList(roles, "RoleID", "RoleName");

        var classes = await _classService.GetAllClassesAsync() ?? new List<Class>();
        ClassOptions = new SelectList(classes, "ClassId", "ClassName");
    }

    public async Task<IActionResult> OnGetAsync()
    {
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
            return Page();
        }

        var newAccount = new Account
        {
            Fullname = Input.Fullname,
            Email = Input.Email,
            Password = Input.Password,
            PhoneNumber = Input.PhoneNumber,
            DateOfBirth = Input.DateOfBirth,
            Address = Input.Address,
            RoleID = Input.RoleID,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow,
            Status = "Active"
        };

        if (ImageFile != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await ImageFile.CopyToAsync(memoryStream);
                newAccount.Image = memoryStream.ToArray();
            }
        }

        try
        {
            var createdAccount = await _accountService.SignUpByAdminAsync(newAccount);
            if (createdAccount != null)
            {
                if (createdAccount.RoleID == 3 && Input.SelectedStudentId.HasValue)
                {
                    await _studentService.UpdateParentForStudentAsync(Input.SelectedStudentId.Value, createdAccount.AccountID);
                }
                TempData["SuccessMessage"] = "Tạo tài khoản mới thành công!";
                return RedirectToPage("Accounts");
            }
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Input.Email", ex.Message);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Đã có lỗi không mong muốn xảy ra: " + ex.Message);
        }

        await PopulateSelectLists();
        return Page();
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BOs.Models;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;

public class CreateAccountModel : PageModel
{
    private readonly IAccountService _accountService;

    public CreateAccountModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public Account Account { get; set; } = new();

    public SelectList RoleOptions { get; set; }

    private async Task LoadSelectLists()
    {
        var roles = await _accountService.GetAllRolesAsync() ?? new List<Role>();
        RoleOptions = new SelectList(roles, "RoleID", "RoleName");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadSelectLists();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return Page();
        }

        Account.CreatedAt = DateTime.UtcNow;
        Account.UpdateAt = DateTime.UtcNow;
        Account.Status = "Active";

        var result = await _accountService.SignUpAsync(Account);

        if (result)
        {
            TempData["SuccessMessage"] = "Tạo tài khoản mới thành công!";
            return RedirectToPage("Accounts");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Email đã tồn tại hoặc có lỗi xảy ra trong quá trình tạo tài khoản.");
            await LoadSelectLists();
            return Page();
        }
    }
}
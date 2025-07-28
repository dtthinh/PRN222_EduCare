using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EditAccountModel : PageModel
{
    private readonly IAccountService _accountService;

    public EditAccountModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public Account Account { get; set; }

    public SelectList RoleOptions { get; set; }
    public SelectList StatusOptions { get; set; }

    private async Task LoadSelectLists()
    {
        var roles = await _accountService.GetAllRolesAsync() ?? new List<Role>();
        RoleOptions = new SelectList(roles, "RoleID", "RoleName", Account?.RoleID);

        var statuses = new List<string> { "Active", "Inactive" };
        StatusOptions = new SelectList(statuses, Account?.Status);
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        Account = await _accountService.GetAccountByIdAsync(id.Value);
        if (Account == null) return NotFound();

        await LoadSelectLists();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Clear();
        if (!ModelState.IsValid)
        {
            await LoadSelectLists();
            return Page();
        }

        await _accountService.UpdateAccountAsync(Account);

        TempData["SuccessMessage"] = "Cập nhật tài khoản thành công!";
        return RedirectToPage("Accounts");
    }
}
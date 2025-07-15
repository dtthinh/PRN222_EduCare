using BOs.Data;
using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services;

public class EditAccountModel : PageModel
{
    private readonly IAccountService _accountService;
    private readonly DataContext _context;

    public EditAccountModel(IAccountService accountService, DataContext context)
    {
        _accountService = accountService;
        _context = context;
    }

    [BindProperty]
    public Account Account { get; set; }

    public SelectList RoleList { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Account = await _accountService.GetAccountByIdAsync(id);
        if (Account == null) return NotFound();

        var roles = await _context.Roles.AsNoTracking().ToListAsync();
        RoleList = new SelectList(roles, "RoleID", "RoleName");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        await _accountService.UpdateAccountAsync(Account);
        return RedirectToPage("Accounts");
    }
}

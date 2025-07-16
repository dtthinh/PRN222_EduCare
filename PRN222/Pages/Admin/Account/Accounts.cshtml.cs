using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

public class AccountsModel : PageModel
{
    private readonly IAccountService _accountService;

    public AccountsModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public List<Account> Accounts { get; set; }

    public async Task OnGetAsync()
    {
        Accounts = await _accountService.GetAllAccountsAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _accountService.DeleteAccountAsync(id);
        return RedirectToPage();
    }
}

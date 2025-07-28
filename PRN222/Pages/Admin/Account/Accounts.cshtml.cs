using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AccountsModel : PageModel
{
    private readonly IAccountService _accountService;

    public AccountsModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public List<Account> Accounts { get; set; } = new List<Account>();

    public async Task OnGetAsync()
    {
        Accounts = await _accountService.GetAllAccountsAsync() ?? new List<Account>();
    }
}
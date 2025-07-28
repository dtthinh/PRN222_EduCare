using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System.Threading.Tasks;

public class DetailsAccountModel : PageModel
{
    private readonly IAccountService _accountService;

    public DetailsAccountModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public Account Account { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Account = await _accountService.GetAccountByIdAsync(id.Value);

        if (Account == null)
        {
            return NotFound();
        }

        return Page();
    }
}
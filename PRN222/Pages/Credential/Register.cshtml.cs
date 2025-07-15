using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace PRN222.Pages.Credential
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountService _accountService;

        public RegisterModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public Account account { get; set; } = new();

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var exists = await _accountService.GetAccountByEmailAsync(account.Email);

            if (exists != null)
            {
                ErrorMessage = "An account with this email already exists.";
                return Page();
            }
            account.RoleID = 3; // Assuming 3 is the RoleID for Parent

            await _accountService.SignUpAsync(account);

            // Optionally, log user in after registration:
            //HttpContext.Session.SetString("UserId", Account.MemberId);

            return RedirectToPage("/Index");
        }
    }
}

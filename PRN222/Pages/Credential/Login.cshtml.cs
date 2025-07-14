using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace PRN222.Pages.Credential
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;

        public LoginModel(IAccountService accountService)
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

            var logedAccount = await _accountService.Login(account.Email, account.Password);

            if (logedAccount != null)
            {
                HttpContext.Session.SetInt32("UserId", logedAccount.AccountID);

                return RedirectToPage("/Index");
            }

            ErrorMessage = "Invalid username or password.";
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return Page();
        }
    }
}

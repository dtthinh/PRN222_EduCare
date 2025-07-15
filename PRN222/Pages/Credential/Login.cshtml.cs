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

                if (logedAccount.RoleID == 1)
                {
                    HttpContext.Session.SetString("Role", "Admin");
                    return RedirectToPage("/Admin/Dashboards/DashBoard");    // Redirect to Admin Home Page
                }
                else if (logedAccount.RoleID == 2)
                {
                    HttpContext.Session.SetString("Role", "Nurse");
                    return RedirectToPage("/Nurse/NurseHomePage");
                }
                else
                {
                    HttpContext.Session.SetString("Role", "Parent");
                    return RedirectToPage("/Index");   // Redirect to Parent Home Page
                }
            }

            ErrorMessage = "Invalid username or password.";
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.Threading.Tasks;

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
        public LoginViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Clear();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loggedInAccount = await _accountService.Login(Input.Email, Input.Password);

            if (loggedInAccount != null)
            {
                if (loggedInAccount.Status != "Active")
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.");
                    return Page();
                }

                HttpContext.Session.SetInt32("UserId", loggedInAccount.AccountID);

                switch (loggedInAccount.RoleID)
                {
                    case 1: // Admin
                        HttpContext.Session.SetString("Role", "Admin");
                        return RedirectToPage("/Admin/Dashboards/Dashboard");
                    case 2: // Nurse
                        HttpContext.Session.SetString("Role", "Nurse");
                        return RedirectToPage("/Nurse/NurseHomePage");
                    case 3: // Parent
                        HttpContext.Session.SetString("Role", "Parent");
                        return RedirectToPage("/Index");
                    default:
                        ModelState.AddModelError(string.Empty, "Vai trò không xác định.");
                        return Page();
                }
            }

            ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không chính xác.");
            return Page();
        }
    }
}
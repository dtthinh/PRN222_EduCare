using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using DAOs;
using System.Threading.Tasks;

namespace PRN222.Pages.Parent
{
    public class ViewHealthRecordModel : PageModel
    {
        public HealthRecord HealthRecord { get; set; }

        public async Task<IActionResult> OnGetAsync(int recordId)
        {
            HealthRecord = await HealthRecordDAO.Instance.GetHealthRecordByIdAsync(recordId);
            return Page();
        }
    }
} 
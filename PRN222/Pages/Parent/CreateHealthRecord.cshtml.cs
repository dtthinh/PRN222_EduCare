using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using DAOs;
using System.Threading.Tasks;
using System.Linq; // Added for .Any()

namespace PRN222.Pages.Parent
{
    public class CreateHealthRecordModel : PageModel
    {
        [BindProperty]
        public HealthRecord HealthRecord { get; set; }
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int studentId)
        {
            Student = await StudentDAO.Instance.GetStudentByIdAsync(studentId);
            if (Student == null)
                return RedirectToPage("/Parent/HealthRecord");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int studentId)
        {
            var student = await StudentDAO.Instance.GetStudentByIdAsync(studentId);
            if (student == null)
                return RedirectToPage("/Parent/HealthRecord");
            int? parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
                return RedirectToPage("/Credential/Login");

            // Check for existing health record
            var existing = await HealthRecordDAO.Instance.GetHealthRecordsByStudentIdAsync(studentId);
            if (existing.Any(hr => hr.ParentId == parentId.Value))
            {
                ModelState.AddModelError("", "A health record for this student already exists.");
                Student = student; // re-populate for the form
                return Page();
            }

            var record = new HealthRecord
            {
                StudentId = studentId,
                ParentId = parentId.Value,
                StudentName = student.Fullname,
                StudentCode = student.StudentCode,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                Note = HealthRecord.Note,
                Height = HealthRecord.Height,
                Weight = HealthRecord.Weight,
                LeftEye = HealthRecord.LeftEye,
                RightEye = HealthRecord.RightEye
            };
            await SaveHealthRecordAsync(record);
            TempData["SuccessMessage"] = "Successfully added a health record.";
            return RedirectToPage("/Parent/HealthRecord");
        }

        private async Task SaveHealthRecordAsync(HealthRecord record)
        {
            await HealthRecordDAO.Instance.CreateHealthRecordAsync(record);
        }
    }
} 
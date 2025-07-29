using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PRN222.Pages.Nurse.Vaccination
{
    public class CreateVaccinationRecordModel : PageModel
    {
        private readonly IVaccinationService _vaccinationService;
        private readonly IStudentService _studentService;
        private readonly IAccountService _accountService;

        public CreateVaccinationRecordModel(IVaccinationService vaccinationService, IStudentService studentService, IAccountService accountService)
        {
            _vaccinationService = vaccinationService;
            _studentService = studentService;
            _accountService = accountService;
        }

        [BindProperty]
        public VaccinationRecord Record { get; set; } = new VaccinationRecord();
        public List<VaccinationCampaign> Campaigns { get; set; } = new();
        public List<Student> Students { get; set; } = new();
        public List<Account> Nurses { get; set; } = new();
        public Account? CurrentNurse { get; set; }
        public string? Message { get; set; }

        public async Task OnGetAsync(int? campaignId, int? studentId)
        {
            Campaigns = await _vaccinationService.GetAllCampaignsAsync();
            Students = await _studentService.GetAllStudentsAsync();
            Nurses = await _accountService.GetActiveNursesAsync();

            if (campaignId.HasValue)
            {
                Record.CampaignId = campaignId.Value;
            }
            if (studentId.HasValue)
            {
                Record.StudentId = studentId.Value;
            }
            var nurseId = HttpContext.Session.GetInt32("UserId");
            if (nurseId.HasValue)
            {
                Record.NurseId = nurseId.Value;
                CurrentNurse = Nurses.FirstOrDefault(n => n.AccountID == nurseId.Value);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Campaigns = await _vaccinationService.GetAllCampaignsAsync();
            Students = await _studentService.GetAllStudentsAsync();
            Nurses = await _accountService.GetActiveNursesAsync();

            // Always set NurseId from session on post
            var nurseId = HttpContext.Session.GetInt32("UserId");
            if (nurseId.HasValue)
            {
                Record.NurseId = nurseId.Value;
                CurrentNurse = Nurses.FirstOrDefault(n => n.AccountID == nurseId.Value);
            }

            // Remove navigation properties from ModelState
            ModelState.Remove("Record.Campaign");
            ModelState.Remove("Record.Student");
            ModelState.Remove("Record.Nurse");

            if (!ModelState.IsValid)
            {
                Message = "Please correct the errors in the form.";
                return Page();
            }

            try
            {
                await _vaccinationService.CreateRecordAsync(Record);
                Message = "Vaccination record created successfully.";
                ModelState.Clear();
                Record = new VaccinationRecord();
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }
            return Page();
        }
    }
}

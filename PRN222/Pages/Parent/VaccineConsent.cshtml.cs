using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace PRN222.Pages.Parent
{
    public class VaccineConsentModel : PageModel
    {
        private readonly IVaccinationService _vaccinationService;

        public VaccineConsentModel(IVaccinationService vaccinationService)
        {
            _vaccinationService = vaccinationService;
        }

        public List<VaccinationConsent> VaccinationConsents { get; set; } = new List<VaccinationConsent>();
        public string Message { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            int? parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Credential/Login");
            }

            VaccinationConsents = await _vaccinationService.GetConsentsByParentIdAsync(parentId.Value);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int? parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Credential/Login");
            }

            var consentId = Request.Form["consentId"].ToString();
            var action = Request.Form["action"].ToString();

            if (string.IsNullOrEmpty(consentId) || string.IsNullOrEmpty(action))
            {
                Message = "Invalid request parameters.";
                VaccinationConsents = await _vaccinationService.GetConsentsByParentIdAsync(parentId.Value);
                return Page();
            }

            if (!int.TryParse(consentId, out int consentIdInt))
            {
                Message = "Invalid consent ID.";
                VaccinationConsents = await _vaccinationService.GetConsentsByParentIdAsync(parentId.Value);
                return Page();
            }

            try
            {
                // Get the consent to verify it belongs to this parent
                var consents = await _vaccinationService.GetConsentsByParentIdAsync(parentId.Value);
                var consent = consents.FirstOrDefault(c => c.ConsentId == consentIdInt);

                if (consent == null)
                {
                    Message = "Consent not found or you don't have permission to modify it.";
                    VaccinationConsents = await _vaccinationService.GetConsentsByParentIdAsync(parentId.Value);
                    return Page();
                }

                // Update the consent based on the action
                bool isAgreed = action.ToLower() == "accept";
                consent.IsAgreed = isAgreed;
                consent.DateConfirmed = DateTime.Now;

                // If rejecting, allow parent to add a note
                if (!isAgreed)
                {
                    var note = Request.Form["note"].ToString();
                    if (!string.IsNullOrEmpty(note))
                    {
                        consent.Note = note;
                    }
                }

                await _vaccinationService.UpdateConsentAsync(consent);

                Message = $"Vaccine consent has been {(isAgreed ? "accepted" : "rejected")} successfully.";
                
                // Refresh the consents list
                VaccinationConsents = await _vaccinationService.GetConsentsByParentIdAsync(parentId.Value);
            }
            catch (Exception ex)
            {
                Message = $"An error occurred while processing your request: {ex.Message}";
                VaccinationConsents = await _vaccinationService.GetConsentsByParentIdAsync(parentId.Value);
            }

            return Page();
        }
    }
} 
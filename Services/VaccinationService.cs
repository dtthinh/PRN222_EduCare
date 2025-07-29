using BOs.Models;
using DAOs;
using Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class VaccinationService : IVaccinationService
    {
        private readonly IVaccinationRepo repo;

        public VaccinationService(IVaccinationRepo repo)
        {
            this.repo = repo;
        }

        public async Task<List<Vaccine>> GetAllVaccinesAsync() => await repo.GetAllVaccinesAsync();
        public async Task<Vaccine?> GetVaccineByIdAsync(int id) => await repo.GetVaccineByIdAsync(id);
        public async Task<Vaccine> CreateVaccineAsync(Vaccine vaccine) => await repo.CreateVaccineAsync(vaccine);
        public async Task<List<VaccinationCampaign>> GetAllCampaignsAsync() => await repo.GetAllCampaignsAsync();
        public async Task<VaccinationCampaign?> GetCampaignByIdAsync(int id) => await repo.GetCampaignByIdAsync(id);
        public async Task<VaccinationCampaign> CreateCampaignAsync(VaccinationCampaign campaign) => await repo.CreateCampaignAsync(campaign);

        public async Task<List<VaccinationConsent>> GetAllConsentsAsync() => await repo.GetAllConsentsAsync();
        public async Task<List<VaccinationConsent>> GetConsentsByCampaignAsync(int campaignId) => await repo.GetConsentsByCampaignAsync(campaignId);
        public async Task<List<VaccinationConsent>> GetConsentsByParentIdAsync(int parentId) => await repo.GetConsentsByParentIdAsync(parentId);
        public async Task<VaccinationConsent?> GetConsentAsync(int campaignId, int studentId, int parentId) => await repo.GetConsentAsync(campaignId, studentId, parentId);
        public async Task<VaccinationConsent?> GetLatestConsentAsync(int campaignId, int studentId) => await repo.GetLatestConsentAsync(campaignId, studentId);
        public async Task<VaccinationConsent> UpdateConsentAsync(VaccinationConsent consent) => await repo.UpdateConsentAsync(consent);
        public async Task<VaccinationConsent> CreateConsentAsync(VaccinationConsent consent) => await repo.CreateConsentAsync(consent);

        public async Task<List<VaccinationRecord>> GetAllRecordsAsync() => await repo.GetAllRecordsAsync();
        public async Task<List<VaccinationRecord>> GetRecordsByCampaignAsync(int campaignId) => await repo.GetRecordsByCampaignAsync(campaignId);
        public async Task<VaccinationRecord?> GetRecordByIdAsync(int id) => await repo.GetRecordByIdAsync(id);
        public async Task<List<VaccinationRecord>> GetRecordsByStudentIdAsync(int studentId) => await repo.GetRecordsByStudentIdAsync(studentId);
        public async Task<VaccinationRecord> CreateRecordAsync(VaccinationRecord record) => await repo.CreateRecordAsync(record);

        public async Task<List<VaccinationFollowUp>> GetFollowUpsByRecordAsync(int recordId) => await repo.GetFollowUpsByRecordAsync(recordId);
        public async Task<VaccinationFollowUp> CreateFollowUpAsync(VaccinationFollowUp followUp) => await repo.CreateFollowUpAsync(followUp);

        public Task<bool> CampaignNameExistsAsync(string name)
            => repo.CampaignNameExistsAsync(name);

        public Task<bool> CampaignTimeConflictAsync(DateTime date)
            => repo.CampaignTimeConflictAsync(date);

        public Task AutoRejectUnconfirmedConsentsAsync(int campaignId, DateTime campaignDate)
            => repo.AutoRejectUnconfirmedConsentsAsync(campaignId, campaignDate);
    }
}
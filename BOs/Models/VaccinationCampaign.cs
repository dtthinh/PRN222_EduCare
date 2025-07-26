using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOs.Models
{
    public class VaccinationCampaign
    {
        public int CampaignId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên chiến dịch.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại vắc xin.")]
        public int VaccineId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày tổ chức.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string? Description { get; set; }

        public Vaccine? Vaccine { get; set; }

        public ICollection<VaccinationConsent> Consents { get; set; } = new List<VaccinationConsent>();
        public ICollection<VaccinationRecord> Records { get; set; } = new List<VaccinationRecord>();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOs.Models
{
    public class VaccinationCampaign
    {
        public int CampaignId { get; set; }

        [Display(Name = "Tên chiến dịch")]
        [Required(ErrorMessage = "Vui lòng nhập tên chiến dịch.")]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "Loại vaccine")]
        [Required(ErrorMessage = "Vui lòng chọn loại vaccine.")]
        public int VaccineId { get; set; }

        [Display(Name = "Ngày tổ chức")]
        [Required(ErrorMessage = "Vui lòng chọn ngày tổ chức.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(1000)]
        public string? Description { get; set; }

        public virtual Vaccine? Vaccine { get; set; }

        public virtual ICollection<VaccinationConsent> Consents { get; set; } = new List<VaccinationConsent>();
        public virtual ICollection<VaccinationRecord> Records { get; set; } = new List<VaccinationRecord>();
    }
}
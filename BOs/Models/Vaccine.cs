using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOs.Models
{
    public class Vaccine
    {
        public int VaccineId { get; set; }

        [Required(ErrorMessage = "Tên vaccine là bắt buộc")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        public string Description { get; set; } = string.Empty;

        public ICollection<VaccinationCampaign> Campaigns { get; set; } = new List<VaccinationCampaign>();
    }
}
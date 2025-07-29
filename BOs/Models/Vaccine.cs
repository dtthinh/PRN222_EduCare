using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOs.Models
{
    public class Vaccine
    {
        public int VaccineId { get; set; }

        [Display(Name = "Tên vaccine")]
        [Required(ErrorMessage = "Tên vaccine là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên vaccine không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string? Description { get; set; }
        public ICollection<VaccinationCampaign> Campaigns { get; set; } = new List<VaccinationCampaign>();
    }
}
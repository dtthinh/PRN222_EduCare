using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOs.Models
{
    public class MedicalEvent
    {
        public int MedicalEventId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn học sinh.")]
        public int StudentId { get; set; }

        [Display(Name = "Y tá phụ trách")]
        [Required(ErrorMessage = "Vui lòng chọn y tá.")]
        public int NurseId { get; set; }

        [Display(Name = "Loại sự kiện")]
        [Required(ErrorMessage = "Loại sự kiện là bắt buộc.")]
        [StringLength(100)]
        public required string Type { get; set; }

        [Display(Name = "Mô tả chi tiết")]
        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [StringLength(1000)]
        public required string Description { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(1000)]
        public string? Note { get; set; }

        [Display(Name = "Ngày xảy ra")]
        [Required(ErrorMessage = "Vui lòng chọn ngày.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual Student Student { get; set; }
        public virtual Account Nurse { get; set; }

        public virtual ICollection<MedicalEventMedication> MedicalEventMedications { get; set; } = new List<MedicalEventMedication>();
        public virtual ICollection<MedicalEventMedicalSupply> MedicalEventMedicalSupplies { get; set; } = new List<MedicalEventMedicalSupply>();
    }
}
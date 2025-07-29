using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [StringLength(200)]
        public string Fullname { get; set; }

        [Display(Name = "Lớp học")]
        [Required(ErrorMessage = "Vui lòng chọn lớp học.")]
        public int ClassId { get; set; }

        [Display(Name = "Mã học sinh")]
        [Required(ErrorMessage = "Vui lòng nhập mã học sinh.")]
        [StringLength(50)]
        public string StudentCode { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
        public string Gender { get; set; }

        [Display(Name = "Phụ huynh")]
        public int? ParentId { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public virtual Account Parent { get; set; }
        public virtual Class Class { get; set; }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1,
    }

}

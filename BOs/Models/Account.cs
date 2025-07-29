using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BOs.Models
{
    public class Account
    {
        public int AccountID { get; set; }

        [Display(Name = "Vai trò")]
        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public int RoleID { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        [StringLength(255)]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [StringLength(150)]
        public string Fullname { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(1000)]
        public string Address { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public byte[]? Image { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ.")]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        [StringLength(50)]
        public string Status { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace BOs.Models
{
    public class Account
    {
        public int AccountID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public int RoleID { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [StringLength(100)]
        public string Fullname { get; set; }

        public string Address { get; set; }

        public byte[]? Image { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc.")]
        public string Status { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
    }
}
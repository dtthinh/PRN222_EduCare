using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Display(Name = "Full Name")]
        public string Fullname { get; set; }
        public string Address { get; set; }
        public byte[]? Image { get; set; } // Thêm dòng này để lưu ảnh đại diện (avatar)

        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Status { get; set; }

        public Role Role { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    }

}

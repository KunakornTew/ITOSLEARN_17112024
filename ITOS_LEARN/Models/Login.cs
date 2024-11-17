using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ITOS_LEARN.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Username must be between 1 and 50 characters.", MinimumLength = 1)]
        public string Username { get; set; }  // ไม่จำเป็นต้องใช้ `required` ในตัวแปรนี้

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }  // เปลี่ยนเป็น nullable เพื่อรองรับค่า null

        [Required]
        [StringLength(50, ErrorMessage = "IpAddress must be between 1 and 50 characters.", MinimumLength = 1)]
        public string IpAddress { get; set; }
    }
}

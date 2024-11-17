using System.ComponentModel.DataAnnotations;

namespace ITOS_LEARN.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Username must be between 1 and 50 characters.", MinimumLength = 1)]  // กำหนดความยาวระหว่าง 1 ถึง 50 ตัวอักษร
        public required string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters.", MinimumLength = 6)]  // กำหนดความยาวระหว่าง 6 ถึง 100 ตัวอักษร
        public required string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]  // ตรวจสอบรูปแบบอีเมล
        [StringLength(100, ErrorMessage = "Email must be between 5 and 100 characters.", MinimumLength = 5)]  // กำหนดความยาวระหว่าง 5 ถึง 100 ตัวอักษร
        public required string Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Role must be 'user' or 'admin'.", MinimumLength = 4)]  // กำหนดความยาวระหว่าง 4 ถึง 10 ตัวอักษร
        public required string Role { get; set; } // 'user' or 'admin'
    }
}

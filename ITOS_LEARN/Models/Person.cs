using System.ComponentModel.DataAnnotations;

namespace ITOS_LEARN.Models
{
    public class Person
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        [MaxLength(30, ErrorMessage = "ความยาวของชื่อต้องไม่เกิน 30 ตัวอักษร")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your lastname")]
        [MaxLength(30, ErrorMessage = "ความยาวของนามสกุลต้องไม่เกิน 30 ตัวอักษร")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your birthday")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [MaxLength(70, ErrorMessage = "ความยาวของอีเมลต้องไม่เกิน 70 ตัวอักษร")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Phone")]
        [MaxLength(15, ErrorMessage = "ความยาวของเบอร์โทรศัพท์ต้องไม่เกิน 15 ตัวอักษร")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your jobposition")]
        [MaxLength(30, ErrorMessage = "ความยาวของตำแหน่งงานต้องไม่เกิน 30 ตัวอักษร")]
        public required string JobPosition { get; set; }
    }
}

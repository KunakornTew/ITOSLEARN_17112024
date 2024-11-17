using Microsoft.AspNetCore.Mvc;

namespace ITOS_LEARN.Models
{
    public class LoginViewModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}


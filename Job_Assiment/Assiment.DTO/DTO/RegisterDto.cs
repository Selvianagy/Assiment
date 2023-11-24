using Assiment.core.Models;
using System.ComponentModel.DataAnnotations;

namespace Assiment.DTO
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ConfirmePassword { get; set; }

    }
}

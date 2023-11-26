
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).{8,}$", ErrorMessage = "Password isn't complex enough")]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]       
        public string DisplayName { get; set; }

        [Required]
        public string Username { get; set; }
    }
}

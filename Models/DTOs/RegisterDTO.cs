
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).{8,}$",
            ErrorMessage = "Password must contain at least 1 Number, 1 uppercase letter, 1 special character and length must be longer than 8")]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]       
        public string DisplayName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }
    }
}

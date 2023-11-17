
using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password isn't complex enough")]
        public string Password { get; set; }

        [Required]       
        public string DisplayName { get; set; }
        public string Username { get; set; }
    }
}

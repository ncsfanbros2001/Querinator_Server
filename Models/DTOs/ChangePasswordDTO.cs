using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string userId { get; set; }

        [Required]
        public string oldPassword { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).{8,}$", ErrorMessage = "Password isn't complex enough")]
        public string newPassword { get; set; }

        [Required]
        public string confirmNewPassword { get; set; }
    }
}

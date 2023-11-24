using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password isn't complex enough")]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}

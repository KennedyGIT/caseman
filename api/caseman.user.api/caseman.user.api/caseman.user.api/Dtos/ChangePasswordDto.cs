using System.ComponentModel.DataAnnotations;

namespace caseman.user.api.Dtos
{
    public class ChangePasswordDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordResetToken { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}

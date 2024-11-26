using caseman.user.api.Helpers;
using System.ComponentModel.DataAnnotations;

namespace caseman.user.api.Dtos
{
    public class CreateUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Institution { get; set; }

        public string Role { get; set; }

        public string Password { get; set; } = new PasswordGenerator().GenerateStrongPassword(8);
    }
}

using Microsoft.AspNetCore.Identity;

namespace core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Institution { get; set; }
        public string Role { get; set; }
        public bool FirstTimeLogin { get; set; } = true;
        public bool IsUserActive { get; set; } = true;
    }
}

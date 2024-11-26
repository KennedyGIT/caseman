using core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    Email = "bob@test.com",
                    FirstName = "bob",
                    LastName = "test",
                    UserName = "bob@test.com",
                    Role = "Admin"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}

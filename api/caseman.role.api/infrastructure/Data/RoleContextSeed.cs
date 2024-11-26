using core.Entities;
using System.Reflection;
using System.Text.Json;

namespace infrastructure.Data
{
    public class RoleContextSeed
    {
        public static async Task SeedAsync(RoleContext context)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!context.Roles.Any())
            {
                var rolesData = File.ReadAllText(path + @"/Data/SeedData/roles.json");
                var roles = JsonSerializer.Deserialize<List<Role>>(rolesData);
                context.Roles.AddRange(roles);
            }
        }
    }
}

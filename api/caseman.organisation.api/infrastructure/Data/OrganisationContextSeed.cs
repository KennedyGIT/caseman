using core.Entities;
using System.Reflection;
using System.Text.Json;

namespace infrastructure.Data
{
    public class OrganisationContextSeed
    {
        public static async Task SeedAsync(OrganisationContext context)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!context.Organisations.Any())
            {
                var organisationsData = File.ReadAllText(path + @"/Data/SeedData/organisation.json");
                var organisations = JsonSerializer.Deserialize<List<Organisation>>(organisationsData);
                context.Organisations.AddRange(organisations);
            }
        }
    }
}

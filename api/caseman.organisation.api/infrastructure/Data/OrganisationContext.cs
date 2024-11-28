using core.Entities;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data
{
    public class OrganisationContext : DbContext
    {
        public OrganisationContext(DbContextOptions<OrganisationContext> options) : base(options)
        {
        }

        public DbSet<Organisation> Organisations { get; set; }
    }
}

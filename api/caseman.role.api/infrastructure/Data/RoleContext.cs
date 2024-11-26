using core.Entities;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data
{
    public class RoleContext : DbContext
    {
        public RoleContext(DbContextOptions<RoleContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
    }
}

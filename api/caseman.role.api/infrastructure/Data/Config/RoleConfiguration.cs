using core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Data.Config
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.Id).IsRequired();
            builder.Property(r => r.RoleName).IsRequired().HasMaxLength(100);
            builder.Property(r => r.CreatedAt).IsRequired();
        }
    }
}

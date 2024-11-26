using core.Entities;
using core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleContext _context;
        public RoleRepository(RoleContext context)
        {
            _context = context;
        }
        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}

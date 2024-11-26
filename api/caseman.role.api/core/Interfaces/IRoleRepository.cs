using core.Entities;

namespace core.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByIdAsync(int id);
        Task<IReadOnlyList<Role>> GetRolesAsync();
    }
}

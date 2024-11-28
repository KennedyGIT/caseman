using core.Entities;

namespace core.Interfaces
{
    public interface IOrganisationRepository
    {
        Task<Organisation> GetOrganisationByIdAsync(int id);
        Task<IReadOnlyList<Organisation>> GetOrganisationsAsync();
    }
}

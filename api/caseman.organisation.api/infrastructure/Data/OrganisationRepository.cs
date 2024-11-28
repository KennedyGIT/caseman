using core.Entities;
using core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly OrganisationContext _context;
        public OrganisationRepository(OrganisationContext context)
        {
            _context = context;
        }
        public async Task<Organisation> GetOrganisationByIdAsync(int id)
        {
            return await _context.Organisations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Organisation>> GetOrganisationsAsync()
        {
            return await _context.Organisations.ToListAsync();
        }
    }
}

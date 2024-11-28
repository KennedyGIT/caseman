using core.Entities;

namespace core.Specifications
{
    public class OrganisationsWithFiltersForCountSpecification : BaseSpecification<Organisation>
    {
        public OrganisationsWithFiltersForCountSpecification(OrganisationSpecParams roleParams): base(x => (string.IsNullOrEmpty(roleParams.Search) || x.OrganisationName.ToLower().Contains(roleParams.Search))){ }
    }
}

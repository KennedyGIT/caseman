using core.Entities;

namespace core.Specifications
{
    public class OrganisationsWithFiltersSpecification : BaseSpecification<Organisation>
    {
        public OrganisationsWithFiltersSpecification(OrganisationSpecParams roleParams) : base(x => (string.IsNullOrEmpty(roleParams.Search) || x.OrganisationName.ToLower().Contains(roleParams.Search))) { }


        public OrganisationsWithFiltersSpecification(int id) : base(x => x.Id == id)
        {
            
        }
    }
}

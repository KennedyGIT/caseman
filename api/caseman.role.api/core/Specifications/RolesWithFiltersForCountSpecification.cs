using core.Entities;

namespace core.Specifications
{
    public class RolesWithFiltersForCountSpecification : BaseSpecification<Role>
    {
        public RolesWithFiltersForCountSpecification(RoleSpecParams roleParams): base(x => (string.IsNullOrEmpty(roleParams.Search) || x.RoleName.ToLower().Contains(roleParams.Search))){ }
    }
}

using core.Entities;

namespace core.Specifications
{
    public class RolesWithFiltersSpecification : BaseSpecification<Role>
    {
        public RolesWithFiltersSpecification(RoleSpecParams roleParams) : base(x => (string.IsNullOrEmpty(roleParams.Search) || x.RoleName.ToLower().Contains(roleParams.Search))) { }


        public RolesWithFiltersSpecification(int id) : base(x => x.Id == id)
        {
            
        }
    }
}

using core.Entities.Identity;

namespace core.Specifications
{
    public class UsersWithInstitutionAndRoleSpecification : BaseSpecification<AppUser>
    {
        public UsersWithInstitutionAndRoleSpecification(UserSpecParams userParams) : base(x => (
        (string.IsNullOrEmpty(userParams.Search))) || x.FirstName.ToLower().Contains(userParams.Search) || x.LastName.ToLower().Contains(userParams.Search) || x.Email.ToLower().Contains(userParams.Search) || x.Institution.ToLower().Contains(userParams.Search) && (!string.IsNullOrEmpty(userParams.Role) || x.Role.ToLower() == userParams.Role))
        {
            AddOrderBy(x => x.FirstName);
            ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1),
                userParams.PageSize);

            if (!string.IsNullOrEmpty(userParams.Sort))
            {
                switch (userParams.Sort)
                {
                    case "InstitutionAsc":
                        AddOrderBy(p => p.Institution);
                        break;
                    case "InstitutionDesc":
                        AddOrderByDescending(p => p.Institution);
                        break;
                    case "RoleAsc":
                        AddOrderBy(p => p.Role);
                        break;
                    case "RoleDesc":
                        AddOrderByDescending(p => p.Role);
                        break;
                    default:
                        AddOrderBy(n => n.FirstName);
                        break;
                }
            }
        }

        public UsersWithInstitutionAndRoleSpecification(string id) : base(x => x.Id == id)
        {

        }


    }
}

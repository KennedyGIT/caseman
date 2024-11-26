using core.Entities.Identity;

namespace core.Specifications
{
    public class UsersWithFiltersForCountSpecification : BaseSpecification<AppUser>
    {
        public UsersWithFiltersForCountSpecification(UserSpecParams userParams) : base(x => (
        (string.IsNullOrEmpty(userParams.Search))) || x.FirstName.ToLower().Contains(userParams.Search) || x.LastName.ToLower().Contains(userParams.Search) || x.Email.ToLower().Contains(userParams.Search) || x.Institution.ToLower().Contains(userParams.Search) && (!string.IsNullOrEmpty(userParams.Role) || x.Role.ToLower() == userParams.Role))
        { }
    }
}

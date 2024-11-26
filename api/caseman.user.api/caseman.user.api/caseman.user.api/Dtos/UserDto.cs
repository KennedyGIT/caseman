namespace caseman.user.api.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FirstTimeLogin { get; set; }
        public string InstitutionName { get; set; }
        public string Role { get; set; }
        public string IsUserActive { get; set; }
        public string Token { get; set; }
    }
}

namespace caseman.user.api.Dtos
{
    public class UpdateUserDto
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Institution { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
    }
}

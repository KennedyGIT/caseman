namespace caseman.user.api.Dtos
{
    public class PasswordResetDto
    {
        public string Email { get; set; }
        public string PasswordResetToken { get; set; }
    }
}

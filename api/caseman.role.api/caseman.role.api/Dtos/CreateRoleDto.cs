using System.ComponentModel.DataAnnotations;

namespace caseman.role.api.Dtos
{
    public class CreateRoleDto
    {

        [Required]
        public string RoleName { get; set; }
        public string? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}

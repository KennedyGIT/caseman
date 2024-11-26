using System.ComponentModel.DataAnnotations;

namespace caseman.role.api.Dtos
{
    public class RoleDtoToReturn
    {
        [Required]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string CreatedAt { get; set; }
        public string LastUpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
    }
}

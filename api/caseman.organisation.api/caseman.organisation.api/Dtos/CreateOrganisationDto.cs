using System.ComponentModel.DataAnnotations;

namespace caseman.organisation.api.Dtos
{
    public class CreateOrganisationDto
    {

        [Required]
        public string OrganisationName { get; set; }
        public string Address { get; set; }
        public string? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}

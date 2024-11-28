using System.ComponentModel.DataAnnotations;

namespace caseman.organisation.api.Dtos
{
    public class OrganisationDtoToReturn
    {
        [Required]
        public int Id { get; set; }
        public string OrganisationName { get; set; }
        public string Address { get; set; }
        public string CreatedAt { get; set; }
        public string LastUpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
    }
}

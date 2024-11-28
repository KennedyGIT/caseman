namespace caseman.organisation.api.Dtos
{
    public class UpdateOrganisationDto
    {
   
        public int Id { get; set; }
        public string OrganisationName { get; set; }
        public string Address { get; set; }
        public string LastUpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}


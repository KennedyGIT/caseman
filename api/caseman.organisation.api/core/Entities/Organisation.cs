namespace core.Entities
{
    public class Organisation : BaseEntity
    {
        public string OrganisationName {  get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedBy { get; set;}

    }
}

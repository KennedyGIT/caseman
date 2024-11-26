namespace core.Entities
{
    public class Role : BaseEntity
    {
        public string RoleName {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

    }
}

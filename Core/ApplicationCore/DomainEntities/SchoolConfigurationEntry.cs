namespace Core.ApplicationCore.DomainEntities;

public class SchoolConfigurationEntry
{
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public bool StaticClassroom { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
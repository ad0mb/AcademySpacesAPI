namespace Core.ApplicationCore.DomainEntities;

public class SchoolEntry
{
    public int SchoolId { get; set; }
    public int OrganizationId { get; set; }
    public string SchoolName { get; set; }
    public string SchoolCountry { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
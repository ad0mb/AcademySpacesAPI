namespace AcademySpacesAPI.ApplicationCore.DomainEntities;

public class ParentEntry
{
    public int ParentId { get; set; }
    public int SchoolId { get; set; }
    public int RoleId { get; set; }
    public string IdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
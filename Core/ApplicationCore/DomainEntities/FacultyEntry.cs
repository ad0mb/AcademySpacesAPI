namespace Core.ApplicationCore.DomainEntities;

public class FacultyEntry
{
    public int FacultyId { get; set; }
    public int SchoolId { get; set; }
    public string? IdentityId { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
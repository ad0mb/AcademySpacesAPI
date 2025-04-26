namespace AcademySpacesAPI.ApplicationCore.DomainEntities;

public class FacultyEntry
{
    public required int FacultyId { get; set; }
    public required int SchoolId { get; set; }
    public string? IdentityId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
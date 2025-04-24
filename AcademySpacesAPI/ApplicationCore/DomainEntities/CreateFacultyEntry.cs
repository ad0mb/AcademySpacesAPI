namespace AcademySpacesAPI.ApplicationCore.DomainEntities;

public class CreateFacultyEntry
{
    public required int SchoolId { get; set; }
    public required string IdentityId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }
}
namespace AcademySpacesAPI.ApplicationCore.DomainEntities;

public class CreateParentEntry
{
    public required int SchoolId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Phone { get; set; }
    public string? Email { get; set; }
}
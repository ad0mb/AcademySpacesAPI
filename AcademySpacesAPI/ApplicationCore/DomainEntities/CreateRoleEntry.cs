namespace AcademySpacesAPI.ApplicationCore.DomainEntities;

public class CreateRoleEntry
{
    public required string RoleName { get; set; }
    public required int SchoolId { get; set; }
}
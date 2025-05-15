namespace AcademySpacesAPI.ApplicationCore.DomainEntities;

public class CreateRolePermissionEntry
{
    public required int RoleId { get; set; }
    public required string PermissionName { get; set; }
    public required bool Create { get; set; }
    public required bool Delete { get; set; }
    public required bool Update { get; set; }
}
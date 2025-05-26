namespace AcademySpacesAPI.ApplicationCore.DomainEntities;

public class RolePermissionEntry
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string PermissionName { get; set; }
    public bool Create { get; set; }
    public bool Delete { get; set; }
    public bool Update { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
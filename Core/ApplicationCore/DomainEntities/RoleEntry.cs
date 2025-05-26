namespace Core.ApplicationCore.DomainEntities;

public class RoleEntry
{
    public int RoleId { get; set; }
    public int SchoolId { get; set; }
    public string RoleName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
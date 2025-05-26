namespace AcademySpacesAPI.ApplicationCore.DomainEntities;

public class FacultyRoleEntry
{
    public int FacultyId { get; set; }
    public int RoleId { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
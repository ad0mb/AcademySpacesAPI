namespace Core.ApplicationCore.DomainEntities;

public class StudentEntry
{
    public int StudentId { get; set; }
    public int SchoolId { get; set; }
    public int YearLevelId { get; set; }
    public string IdentityId { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    
    public List<int> ParentIds { get; set; }
}
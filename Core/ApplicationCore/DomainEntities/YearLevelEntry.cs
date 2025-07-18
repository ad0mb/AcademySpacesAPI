namespace Core.ApplicationCore.DomainEntities;

public class YearLevelEntry
{
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public string YearLevelName { get; set; }
    public string YearLevelCode { get; set; }
    public string? Description { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
}
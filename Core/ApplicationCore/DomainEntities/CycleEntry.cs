namespace Core.ApplicationCore.DomainEntities;

public class CycleEntry
{
    public int CycleId { get; set; }
    public int SchoolId { get; set; }
    public bool IsActive { get; set; }
    public string CycleName { get; set; }
    public string Code { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    public bool IsExpired { get; set; }
}
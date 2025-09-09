namespace Core.ApplicationCore.DomainEntities;

public class PeriodEntry
{
    public int PeriodId { get; set; }
    public int CycleId { get; set; }
    public int? TeacherId { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; }
    public string? Location { get; set; }
    public List<PeriodScheduleEntry> PeriodSchedule { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    
    public CourseEntry Course { get; set; }
    public FacultyEntry? Teacher { get; set; }
}
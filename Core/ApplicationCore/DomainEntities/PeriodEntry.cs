namespace Core.ApplicationCore.DomainEntities;

public class PeriodEntry
{
    public int PeriodId { get; set; }
    public int? TeacherId { get; set; }
    public int CourseId { get; set; }
    public string? Location { get; set; }
    public int Capacity { get; set; }
    public int? DayOfWeek { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    
    public CourseEntry Course { get; set; }
    public FacultyEntry? Teacher { get; set; }
}
namespace Core.ApplicationCore.DomainEntities;

public class PeriodScheduleEntry
{
    public int Id { get; set; }
    public int PeriodId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
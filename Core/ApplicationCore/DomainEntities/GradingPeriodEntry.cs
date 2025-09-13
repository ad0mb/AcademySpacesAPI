namespace Core.ApplicationCore.DomainEntities;

public class GradingPeriodEntry
{
    public int GradingPeriodId { get; set; }
    public int CycleId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
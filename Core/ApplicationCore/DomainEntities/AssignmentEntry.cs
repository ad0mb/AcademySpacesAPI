using Core.ApplicationCore.Enums;

namespace Core.ApplicationCore.DomainEntities;

public class AssignmentEntry
{
    public int AssignmentId { get; set; }
    public int PeriodId { get; set; }
    public AssignmentType AssignmentType { get; set; }
    public string AssignmentName { get; set; } = null!;
    public int MaxScore { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
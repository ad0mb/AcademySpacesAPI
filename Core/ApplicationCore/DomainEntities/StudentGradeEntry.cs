namespace Core.ApplicationCore.DomainEntities;

public class StudentGradeEntry
{
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    public int Score { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}
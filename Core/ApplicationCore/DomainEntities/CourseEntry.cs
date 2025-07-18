namespace Core.ApplicationCore.DomainEntities;

public class CourseEntry
{
    public int CourseId { get; set; }
    public int schoolId { get; set; }
    public string CourseName { get; set; }
    public string CourseCode { get; set; }
    public string? CourseDescription { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}
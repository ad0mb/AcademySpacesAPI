namespace Core.ApplicationCore.DomainEntities;

public class ClassroomEntry
{
    public int ClassroomId { get; set; }
    public int CycleId { get; set; }
    public int? ClassroomTeacherId { get; set; }
    public string ClassroomName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    
    public int NumberOfStudents { get; set; }
    public string ClassroomTeacherName { get; set; }
}
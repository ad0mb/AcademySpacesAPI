using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IStudentGradeRepository
{
    Task<List<StudentGradeEntry>> GetStudentsGradesAsync(int cycleId, int assignmentId, int periodId);
    Task CreateStudentGradesAsync(int assignmentId, int periodId, List<StudentGradeEntry> entries);
    Task UpdateStudentGradesAsync(int assignmentId, int periodId, List<StudentGradeEntry> entries);
    Task DeleteStudentGradesAsync(int assignmentId, int periodId, List<int> studentIds);
}
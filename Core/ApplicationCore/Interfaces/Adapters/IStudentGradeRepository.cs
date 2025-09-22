using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IStudentGradeRepository
{
    Task<List<StudentGradeEntry>> GetStudentsGradesAsync(int cycleId, int assignmentId, int periodId);
    Task SetStudentsGradesAsync(int schoolId, int cycleId, int periodId, List<StudentGradeEntry> gradeEntries);
}
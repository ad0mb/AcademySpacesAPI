using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IUpdateStudentGradesUseCase
{
    Task UpdateStudentGradesAsync(int assignmentId, int periodId, List<StudentGradeEntry> entries);
}

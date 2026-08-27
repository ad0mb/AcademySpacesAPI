using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface ICreateStudentGradesUseCase
{
    Task CreateStudentGradesAsync(int assignmentId, int periodId, List<StudentGradeEntry> entries);
}

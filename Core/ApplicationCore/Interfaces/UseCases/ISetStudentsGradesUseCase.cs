using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface ISetStudentsGradesUseCase
{
    Task SetStudentsGradesAsync(int schoolId, int cycleId, int periodId, List<StudentGradeEntry> gradeEntries);
}
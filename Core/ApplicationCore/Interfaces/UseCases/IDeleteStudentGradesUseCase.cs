namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IDeleteStudentGradesUseCase
{
    Task DeleteStudentGradesAsync(int assignmentId, int periodId, List<int> studentIds);
}

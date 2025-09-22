namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IDeleteStudentUseCase
{
    Task DeleteStudentAsync(int schoolId, int studentId);
}
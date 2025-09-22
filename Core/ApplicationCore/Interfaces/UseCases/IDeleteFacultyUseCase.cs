namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IDeleteFacultyUseCase
{
    Task DeleteFacultyAsync(int schoolId, int facultyId);
}
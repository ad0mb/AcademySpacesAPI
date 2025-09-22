namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IDeleteCourseUseCase
{
    Task DeleteCourseAsync(int schoolId, int courseId);
}
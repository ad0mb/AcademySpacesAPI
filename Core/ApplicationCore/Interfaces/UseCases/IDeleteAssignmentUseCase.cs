namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IDeleteAssignmentUseCase
{
    Task DeleteAssignmentAsync(int assignmentId, int periodId);
}

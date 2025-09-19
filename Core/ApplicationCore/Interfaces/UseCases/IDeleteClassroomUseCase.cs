namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IDeleteClassroomUseCase
{
    Task DeleteClassroomAsync(int cycleId, int classroomId);
}
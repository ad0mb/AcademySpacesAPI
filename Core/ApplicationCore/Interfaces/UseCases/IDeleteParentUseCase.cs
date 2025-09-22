namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IDeleteParentUseCase
{
    Task DeleteParentAsync(int schoolId, int parentId);
}
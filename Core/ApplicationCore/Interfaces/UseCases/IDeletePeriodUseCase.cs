namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IDeletePeriodUseCase
{
    Task DeletePeriodAsync(int cycleId, int periodId);
}
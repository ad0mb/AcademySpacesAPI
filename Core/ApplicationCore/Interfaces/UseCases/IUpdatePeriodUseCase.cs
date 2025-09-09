using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IUpdatePeriodUseCase
{
    Task UpdatePeriodAsync(PeriodEntry request, List<int> periodScheduleEntriesToDelete);
}
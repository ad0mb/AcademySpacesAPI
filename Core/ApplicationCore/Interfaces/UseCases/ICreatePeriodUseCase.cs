using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface ICreatePeriodUseCase
{
    Task CreatePeriodAsync(PeriodEntry request);
}
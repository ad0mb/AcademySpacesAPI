using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class CreatePeriodUseCase : ICreatePeriodUseCase
{
    public readonly IPeriodsRepository _periodsRepository;

    public CreatePeriodUseCase(IPeriodsRepository periodsRepository)
    {
        _periodsRepository = periodsRepository;
    }

    public async Task CreatePeriodAsync(PeriodEntry request)
    {
        await _periodsRepository.CreatePeriodAsync(request);
    }
}
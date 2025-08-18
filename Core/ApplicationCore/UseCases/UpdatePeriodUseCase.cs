using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class UpdatePeriodUseCase : IUpdatePeriodUseCase
{
    
private readonly IPeriodsRepository _periodsRepository;

    public UpdatePeriodUseCase(IPeriodsRepository periodsRepository)
    {
        _periodsRepository = periodsRepository;
    }

    public async Task UpdatePeriodAsync(PeriodEntry request)
    {
        await _periodsRepository.UpdatePeriodAsync(request);
    }
}
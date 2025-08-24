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

    public async Task UpdatePeriodAsync(PeriodEntry request, List<int> periodScheduleEntriesToDelete)
    {
        await _periodsRepository.BulkDeletePeriodScheduleEntriesAsync(periodScheduleEntriesToDelete);
        
        await _periodsRepository.UpdatePeriodAsync(request);
    }
}
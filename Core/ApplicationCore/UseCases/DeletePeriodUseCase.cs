using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class DeletePeriodUseCase : IDeletePeriodUseCase
{
    
    private readonly IPeriodsRepository _periodRepository;
    
    public DeletePeriodUseCase(IPeriodsRepository periodRepository)
    {
        _periodRepository = periodRepository;
    }

    public async Task DeletePeriodAsync(int schoolId, int periodId)
    {
        await _periodRepository.DeletePeriodAsync(schoolId, periodId);
    }
}
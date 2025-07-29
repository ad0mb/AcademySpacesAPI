using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetPeriodsUseCase : IGetPeriodsUseCase
{
    
    private readonly IPeriodsRepository _periodsRepository;

    public GetPeriodsUseCase(IPeriodsRepository periodsRepository)
    {
        _periodsRepository = periodsRepository;
    }
    
    public async Task<List<PeriodEntry>> GetPeriodsBySchoolAsync(int schoolId)
    {
        var periods = await _periodsRepository.GetPeriodsBySchoolIdAsync(schoolId);
        
        return periods;
    }
    
}
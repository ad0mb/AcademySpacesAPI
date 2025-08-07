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
    
    public async Task<(List<PeriodEntry> periodsList, int totalCount)> GetPeriodsBySchoolAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm, int facultyId, int courseId, TimeOnly? startTime, TimeOnly? endTime)
    {
        var (periodsList, totalCount) = await _periodsRepository.GetPeriodsBySchoolIdAsync(schoolId, pageSize, pageNumber, searchTerm, facultyId, courseId, startTime, endTime);
        
        return (periodsList, totalCount);
    }
    
}
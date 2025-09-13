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
    
    public async Task<(List<PeriodEntry> periodsList, int totalCount)> GetPeriodsBySchoolAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm, int facultyId, int courseId, TimeOnly? startTime, TimeOnly? endTime, int[] dayOfWeek, bool excludeClassroomId = false, bool onlyScheduled = false)
    {
        var (periodsList, totalCount) = await _periodsRepository.GetPeriodsBySchoolIdAsync(schoolId, cycleId, pageSize, pageNumber, searchTerm, facultyId, courseId, startTime, endTime, dayOfWeek, excludeClassroomId, onlyScheduled);
        
        return (periodsList, totalCount);
    }
    
}
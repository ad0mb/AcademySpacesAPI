using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetClassroomScheduleUseCase : IGetClassroomScheduleUseCase
{
    
    private readonly IPeriodsRepository _periodsRepository;

    public GetClassroomScheduleUseCase(IPeriodsRepository periodsRepository)
    {
        _periodsRepository = periodsRepository;
    }

    public async Task<List<PeriodEntry>> GetClassroomScheduleAsync(int classroomId, int cycleId)
    {
        var periodsList = await _periodsRepository.GetPeriodsByClassroomIdAsync(classroomId, cycleId);
        
        return periodsList;
    }
}
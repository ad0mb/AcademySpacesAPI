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

    //TODO: Look into making periods repository basic get method retrieve the periods by classrrom instead of an entirely new method, refer to how classroom roster is retreived from basic general get students method using filtering and 0/null entries for undesired fields and filters
    public async Task<List<PeriodEntry>> GetClassroomScheduleAsync(int classroomId, int cycleId)
    {
        var periodsList = await _periodsRepository.GetPeriodsByClassroomIdAsync(classroomId, cycleId);
        
        return periodsList;
    }
}
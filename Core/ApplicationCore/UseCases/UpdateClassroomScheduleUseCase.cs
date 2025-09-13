using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class UpdateClassroomScheduleUseCase : IUpdateClassroomScheduleUseCase
{
    
    private readonly IClassroomRepository _classroomRepository;

    public UpdateClassroomScheduleUseCase(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public async Task UpdateClassroomScheduleAsync(int cycleId, int classroomId, List<int> periodId)
    {
        await _classroomRepository.UpdateClassroomScheduleAsync(cycleId, classroomId, periodId);
    }
}
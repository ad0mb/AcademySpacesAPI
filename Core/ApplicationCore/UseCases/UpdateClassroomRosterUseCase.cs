using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class UpdateClassroomRosterUseCase : IUpdateClassroomRosterUseCase
{

    private readonly IClassroomRepository _classroomRepository;

    public UpdateClassroomRosterUseCase(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public async Task UpdateClassroomRoster(int schoolId, int cycleId, int classroomId, List<int> studentIds)
    {
        await _classroomRepository.UpdateClassroomRosterAsync(schoolId, cycleId, classroomId, studentIds);
    }
}
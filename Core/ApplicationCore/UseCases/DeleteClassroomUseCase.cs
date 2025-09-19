using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class DeleteClassroomUseCase : IDeleteClassroomUseCase
{
    
    private readonly IClassroomRepository _classroomRepository;
    
    public DeleteClassroomUseCase(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public async Task DeleteClassroomAsync(int cycleId, int classroomId)
    {
        await _classroomRepository.DeleteClassroomAsync(cycleId, classroomId);
    }
}
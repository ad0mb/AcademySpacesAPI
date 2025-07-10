using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetClassroomsUseCase : IGetClassroomsUseCase
{
    
    private readonly IClassroomRepository _classroomRepository;

    public GetClassroomsUseCase(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public async Task<List<ClassroomEntry>> GetClassroomsAsync(int schoolId)
    {
        var classrooms = await _classroomRepository.GetClassroomsAsync(schoolId);

        return classrooms;
    }
}
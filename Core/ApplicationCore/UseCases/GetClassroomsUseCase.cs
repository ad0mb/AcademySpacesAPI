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
    
    public async Task<(List<ClassroomEntry> classrooms, int totalCount)> GetClassroomsAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {
        var (classrooms, totalCount) = await _classroomRepository.GetClassroomsAsync(schoolId, pageSize, pageNumber, searchTerm);
        
        return (classrooms, totalCount);
    }
}
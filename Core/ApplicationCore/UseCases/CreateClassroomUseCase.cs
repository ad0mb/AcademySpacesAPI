using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;

namespace Core.ApplicationCore.UseCases;

public class CreateClassroomUseCase : ICreateClassroomUseCase
{
    
    private readonly IClassroomRepository _classroomRepository;
    
    public CreateClassroomUseCase(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public async Task CreateClassroomAsync(ClassroomEntry request)
    {
        await _classroomRepository.CreateClassroomAsync(request);
    }
}
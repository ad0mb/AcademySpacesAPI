using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class CreateParentUseCase : ICreateParentUseCase
{
    private readonly IParentRepository _parentRepository;
    private readonly IFacultyRepository _facultyRepository;
    
    public CreateParentUseCase(IParentRepository parentRepository, IFacultyRepository facultyRepository)
    {
        _parentRepository = parentRepository;
        _facultyRepository = facultyRepository;
    }
    
    public async Task CreateParentAsync(ParentEntry request, int schoolId)
    {
        request.SchoolId = schoolId;
        
        await _parentRepository.CreateParentAsync(request);
    }
}
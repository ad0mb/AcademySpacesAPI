using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetParentsUseCase : IGetParentsUseCase
{
    
    private readonly IParentRepository _parentRepository;

    public GetParentsUseCase(IParentRepository parentRepository)
    {
        _parentRepository = parentRepository;
    }

    public async Task<List<ParentEntry>> GetParentsAsync(int schoolId)
    {
        var parent = await _parentRepository.GetParentsBySchoolIdAsync(schoolId);

        return parent;
    }
}
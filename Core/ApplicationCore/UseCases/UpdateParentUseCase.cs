using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class UpdateParentUseCase : IUpdateParentUseCase
{
    
    private readonly IParentRepository _parentRepository;
    
    public UpdateParentUseCase(IParentRepository parentRepository)
    {
        
        _parentRepository = parentRepository;
    }

    public async Task UpdateParentAsync(ParentEntry parent)
    {
        await _parentRepository.UpdateParentAsync(parent);
    }
}
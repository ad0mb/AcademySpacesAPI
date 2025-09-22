using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class DeleteParentUseCase : IDeleteParentUseCase
{
    
    private readonly IParentRepository _parentRepository;
    
    public DeleteParentUseCase(IParentRepository parentRepository)
    {
        _parentRepository = parentRepository;
    }

    public async Task DeleteParentAsync(int schoolId, int parentId)
    {
        await _parentRepository.DeleteParentAsync(schoolId, parentId);
    }
}
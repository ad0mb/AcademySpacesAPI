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

    public async Task<(List<ParentEntry> parentList, int totalCount)> GetParentsAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {
        var (parent, totalCount) = await _parentRepository.GetParentsBySchoolIdAsync(schoolId, pageSize, pageNumber, searchTerm);

        return (parent, totalCount);
    }
}
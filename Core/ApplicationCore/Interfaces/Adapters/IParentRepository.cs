using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IParentRepository
{
    Task CreateParentAsync(ParentEntry parent);
    Task<ParentEntry> GetParentByParentIdAsync(int parentId);
    
    Task<(List<ParentEntry> parentList, int totalCount)> GetParentsBySchoolIdAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm);
    Task UpdateParentAsync(ParentEntry parent);
}
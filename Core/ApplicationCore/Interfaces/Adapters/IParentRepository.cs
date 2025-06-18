using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IParentRepository
{
    Task CreateParentAsync(ParentEntry parent);
    Task<ParentEntry> GetParentByParentIdAsync(int parentId);
    
    Task<List<ParentEntry>> GetParentsBySchoolIdAsync(int schoolId);
    Task UpdateParentAsync(ParentEntry parent);
}
using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IParentRepository
{
    Task CreateParentAsync(ParentEntry parent);
}
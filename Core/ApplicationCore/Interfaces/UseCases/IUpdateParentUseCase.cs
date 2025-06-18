using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IUpdateParentUseCase
{
    Task UpdateParentAsync(ParentEntry parent);
}
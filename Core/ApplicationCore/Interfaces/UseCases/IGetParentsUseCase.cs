using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetParentsUseCase
{
    Task<List<ParentEntry>> GetParentsAsync(int schoolId);
}
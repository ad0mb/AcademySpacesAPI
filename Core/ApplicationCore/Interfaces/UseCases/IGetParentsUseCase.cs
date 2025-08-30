using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetParentsUseCase
{
    Task<(List<ParentEntry> parentList, int totalCount)> GetParentsAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm);
}
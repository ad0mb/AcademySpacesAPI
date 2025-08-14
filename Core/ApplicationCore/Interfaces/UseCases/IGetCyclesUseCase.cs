using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetCyclesUseCase
{
    Task<(List<CycleEntry> cyclesList, int totalCount)> GetCyclesAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm);
}
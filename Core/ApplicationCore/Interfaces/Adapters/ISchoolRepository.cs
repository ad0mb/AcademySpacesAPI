using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface ISchoolRepository
{
    Task<int[]> CreateSchoolAsync(SchoolEntry school);
    Task<SchoolConfigurationEntry> GetSchoolConfigurationAsync(int schoolId);
    Task<(List<CycleEntry> cyclesList, int totalCount)> GetCyclesAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm);
    Task<int?> GetActiveCycleIdAsync(int schoolId);
    Task<bool> IsCycleValidAsync(int schoolId, int cycleId);
    Task CreateCycleAsync(CycleEntry cycle);
}
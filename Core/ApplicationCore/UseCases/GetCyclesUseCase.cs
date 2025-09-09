using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetCyclesUseCase : IGetCyclesUseCase
{

    private readonly ISchoolRepository _schoolRepository;

    public GetCyclesUseCase(ISchoolRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }
    
    public async Task<(List<CycleEntry> cyclesList, int totalCount)> GetCyclesAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {
        var (cycles, totalCount) = await _schoolRepository.GetCyclesAsync(schoolId, pageSize, pageNumber, searchTerm);

        return (cycles, totalCount);
    }
}
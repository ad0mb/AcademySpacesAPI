using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetYearLevelHierarchyUseCase : IGetYearLevelHierarchyUseCase
{
    
    private readonly IYearLevelRepository _yearLevelRepository;
    
    public GetYearLevelHierarchyUseCase(IYearLevelRepository yearLevelRepository)
    {
        _yearLevelRepository = yearLevelRepository;
    }
    
    public async Task<List<YearLevelEntry>> GetYearLevelHierarchyAsync(int schoolId)
    {
        var yearLevels = await _yearLevelRepository.GetYearLevelsBySchoolIdAsync(schoolId);

        return yearLevels;
    }
}
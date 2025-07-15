using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class SetYearLevelHierarchyUseCase : ISetYearLevelHierarchyUseCase
{
    
    private readonly IYearLevelRepository _yearLevelRepository;
    
    public SetYearLevelHierarchyUseCase(IYearLevelRepository yearLevelRepository)
    {
        _yearLevelRepository = yearLevelRepository;
    }
    
    public async Task SetYearLevelHierarchyAsync(List<YearLevelEntry> yearLevelEntries, List<int> yearLevelsToDelete)
    {
        await _yearLevelRepository.DeleteYearLevelsAsync(yearLevelsToDelete);

        await _yearLevelRepository.BulkUpdateOrInsertYearLevelHeirarchyAsync(yearLevelEntries);
    }
}
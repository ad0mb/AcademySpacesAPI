using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface ISetYearLevelHierarchyUseCase
{
    Task SetYearLevelHierarchyAsync(List<YearLevelEntry> yearLevelEntries, List<int> yearLevelsToDelete);
}
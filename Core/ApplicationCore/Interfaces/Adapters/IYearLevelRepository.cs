using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IYearLevelRepository
{
    Task<List<YearLevelEntry>> GetYearLevelsBySchoolIdAsync(int schoolId);
    Task BulkUpdateOrInsertYearLevelHeirarchyAsync(List<YearLevelEntry> request);
    Task DeleteYearLevelsAsync(List<int> request);
}
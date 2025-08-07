using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IPeriodsRepository
{
    Task<(List<PeriodEntry> periodsList, int totalCount)> GetPeriodsBySchoolIdAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm, int facultyId, int courseId, TimeOnly? startTime, TimeOnly? endTime);
}
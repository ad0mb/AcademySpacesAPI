using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IPeriodsRepository
{
    Task<(List<PeriodEntry> periodsList, int totalCount)> GetPeriodsBySchoolIdAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm, int facultyId, int courseId, TimeOnly? startTime, TimeOnly? endTime, int dayOfWeek, bool onlyScheduled = false);
    Task CreatePeriodAsync(PeriodEntry request);
    Task UpdatePeriodAsync(PeriodEntry request);
}
using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetPeriodsUseCase
{
    Task<(List<PeriodEntry> periodsList, int totalCount)> GetPeriodsBySchoolAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm, int facultyId, int courseId, TimeOnly? startTime, TimeOnly? endTime);
}
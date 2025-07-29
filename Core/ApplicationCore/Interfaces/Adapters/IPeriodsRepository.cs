using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IPeriodsRepository
{
    Task<List<PeriodEntry>> GetPeriodsBySchoolIdAsync(int schoolId);
}
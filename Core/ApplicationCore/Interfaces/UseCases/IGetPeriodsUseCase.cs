using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetPeriodsUseCase
{
    Task<List<PeriodEntry>> GetPeriodsBySchoolAsync(int schoolId);
}
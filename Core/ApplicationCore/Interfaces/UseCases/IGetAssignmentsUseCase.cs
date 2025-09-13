using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetAssignmentsUseCase
{
    Task<List<AssignmentEntry>> GetAssignmentsByPeriodIdAsync(int cycleId, int periodId);
}
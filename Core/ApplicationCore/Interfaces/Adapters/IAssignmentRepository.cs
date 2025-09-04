using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IAssignmentRepository
{
    Task<List<AssignmentEntry>> GetAssignmentsByPeriodIdAsync(int cycleId, int periodId);
}
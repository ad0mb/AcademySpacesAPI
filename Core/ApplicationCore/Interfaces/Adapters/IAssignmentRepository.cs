using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IAssignmentRepository
{
    Task<List<AssignmentEntry>> GetAssignmentsByPeriodIdAsync(int cycleId, int periodId);
    Task CreateAssignmentAsync(AssignmentEntry request);
    Task UpdateAssignmentAsync(AssignmentEntry request);
    Task DeleteAssignmentAsync(int assignmentId, int periodId);
}
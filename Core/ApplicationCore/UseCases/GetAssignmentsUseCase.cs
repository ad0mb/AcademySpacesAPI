using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetAssignmentsUseCase : IGetAssignmentsUseCase
{
    
    private readonly IAssignmentRepository _assignmentRepository;

    public GetAssignmentsUseCase(IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    public async Task<List<AssignmentEntry>> GetAssignmentsByPeriodIdAsync(int cycleId, int periodId)
    {
        var assignments = await _assignmentRepository.GetAssignmentsByPeriodIdAsync(cycleId, periodId);

        return assignments;
    }
}
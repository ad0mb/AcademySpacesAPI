using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class DeleteAssignmentUseCase : IDeleteAssignmentUseCase
{
    private readonly IAssignmentRepository _assignmentRepository;

    public DeleteAssignmentUseCase(IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    public async Task DeleteAssignmentAsync(int assignmentId, int periodId)
    {
        await _assignmentRepository.DeleteAssignmentAsync(assignmentId, periodId);
    }
}

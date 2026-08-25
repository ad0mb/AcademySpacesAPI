using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class UpdateAssignmentUseCase : IUpdateAssignmentUseCase
{
    private readonly IAssignmentRepository _assignmentRepository;

    public UpdateAssignmentUseCase(IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    public async Task UpdateAssignmentAsync(AssignmentEntry request)
    {
        await _assignmentRepository.UpdateAssignmentAsync(request);
    }
}

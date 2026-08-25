using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class CreateAssignmentUseCase : ICreateAssignmentUseCase
{
    private readonly IAssignmentRepository _assignmentRepository;

    public CreateAssignmentUseCase(IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    public async Task CreateAssignmentAsync(AssignmentEntry request)
    {
        await _assignmentRepository.CreateAssignmentAsync(request);
    }
}

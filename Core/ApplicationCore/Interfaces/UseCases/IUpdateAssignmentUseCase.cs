using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IUpdateAssignmentUseCase
{
    Task UpdateAssignmentAsync(AssignmentEntry request);
}

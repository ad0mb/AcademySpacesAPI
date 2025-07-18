using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface ICreateClassroomUseCase
{
    Task CreateClassroomAsync(ClassroomEntry request);
}